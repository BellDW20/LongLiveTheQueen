using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[System.Serializable]
public class PlayerController : MonoBehaviour {

    private static readonly Color INVULN_COLOR = new Color(1, 0.5f, 0.5f, 0.75f);

    public float MOVE_SPEED;
    public float DASH_SPEED;
    public float DASH_TIME;
    public float INVULN_TIME;
    public float DEATH_INVULN_TIME;
    public GameObject DASH_PARTICLES;
    [SerializeField] private Gun _primaryGun;
    [SerializeField] private Gun _specialGun;

    private bool _isDashing = false;
    private float _dashStartTime;
    private float _timeLastDamaged, _timeLastRespawned;

    private Rigidbody2D _rbody;
    private CircleCollider2D _collider;
    private Transform _transform;
    private Animations _animations;
    private SpriteRenderer _spr;
    private int _playerNumber, _joystickNumber;
    private PlayerInfo _playerInfo;
    private Camera _cam;

    private Vector2 _lookDirection;

    public virtual void Start() {
        _rbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _animations = new Animations(GetComponent<Animator>(), "Stand");
        _spr = GetComponent<SpriteRenderer>();
        _transform = transform;
        _playerNumber = LevelManagerScript.GetPlayerNumber(gameObject);
        _joystickNumber = InputManager.GetPlayerAssignedJoystick(_playerNumber);
        _playerInfo = LevelManagerScript.pInfos[_playerNumber];
        _cam = Camera.main;
        _primaryGun.Init();
        _specialGun.Init();
        MSMScript.RegisterPlayer(gameObject);
    }

    public virtual void Update() {
        if (IsDead()) { return; }

        if (IsInvulnerable()) {
            _spr.color = INVULN_COLOR;
        }
        else {
            _spr.color = Color.white;
        }

        if (InputManager.GetBackInput(_joystickNumber)) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "MainMenu");
        }

        if (!_isDashing && InputManager.GetDashInput(_joystickNumber) && _rbody.velocity.magnitude > 0.1f) {
            _rbody.velocity = _rbody.velocity.normalized * DASH_SPEED;
            _isDashing = true;
            _dashStartTime = Time.time;
            SoundManager.PlaySFX(SFX.DASH);
            Instantiate(DASH_PARTICLES, _transform.position, Quaternion.identity);
        }

        float timeSinceDash = Time.time - _dashStartTime;
        if (timeSinceDash >= DASH_TIME) {
            _isDashing = false;
        }
        else {
            float factor = timeSinceDash / DASH_TIME;
            _rbody.velocity = (_rbody.velocity.normalized * Mathf.Lerp(DASH_SPEED, MOVE_SPEED, factor));
        }

        Vector2 input = new Vector2(InputManager.GetHorizontalInput(_joystickNumber), InputManager.GetVerticalInput(_joystickNumber));

        if (_rbody.velocity.magnitude > 0.1f && input.magnitude > 0.1f) {
            _animations.SetAnimation("Walk");
        }
        else {
            _animations.SetAnimation("Stand");
        }

        Vector2 tempLookDir = InputManager.GetAimInput(_cam, _rbody.position, _joystickNumber);
        if (tempLookDir.magnitude != 0) {
            _lookDirection = tempLookDir;
            UpdateLookDirection();
        }

        HandleShooting();

        if (_isDashing) return;

        _rbody.velocity = (input * MOVE_SPEED);
    }

    private void HandleShooting() {
        if (InputManager.GetFireInput(_joystickNumber)) {
            _primaryGun.Shoot(_transform.position, _playerNumber, _lookDirection);
        } else if (InputManager.GetReloadInput(_joystickNumber)) {
            _primaryGun.Reload();
        }

        //Updates amount of ammo player has
        GameHUDScript.UpdatePlayerAmmoVisual(_playerNumber, _primaryGun);

        //Check if player is shooting special and if the special cooldown is ready
        if (InputManager.GetSpecialInput(_joystickNumber)) {
            HandleSpecial();
        }

        //Updates the "Special Ready" text
        GameHUDScript.UpdatePlayerSpecialVisual(_playerNumber, !_specialGun.IsReloading());
    }

    private void UpdateLookDirection() {
        //update rotation to follow looking direction
        _transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, 180 * Mathf.Atan2(_lookDirection.y, _lookDirection.x) / Mathf.PI - 90)
        );
    }

    public virtual void HandleSpecial()
    {
        _specialGun.Shoot(_transform.position, _playerNumber, _lookDirection);
    }

    public Vector2 GetLookDirection() {
        return _lookDirection;
    }

    public int GetPlayerNumber() {
        return _playerNumber;
    }

    public bool IsDead() {
        return _playerInfo.health <= 0;
    }
    public bool IsInvulnerable() {
        return (Time.time - _timeLastDamaged) < INVULN_TIME || (Time.time - _timeLastRespawned) < DEATH_INVULN_TIME;
    }

    public void Damage(float amt) {
        if(!IsInvulnerable()) {
            _playerInfo.health -= amt;
            if (_playerInfo.health <= 0) {
                Die();
            } else {
                _timeLastDamaged = Time.time;
            }
            SoundManager.PlaySFX(SFX.PLAYER_GRUNT);
            GameHUDScript.UpdatePlayerHealthVisual(_playerNumber);
        }
    }

    private void Die() {
        _playerInfo.stock--;
        GameHUDScript.UpdatePlayerStockVisual(_playerNumber);

        _spr.enabled = false;
        _rbody.simulated = false;
        _collider.enabled = false;

        if (_playerInfo.stock>=0) {
            Invoke("Respawn", 3);
        } else if (LevelManagerScript.IsGameOver()) {
            Invoke("GameOver", 3);
        }
    }

    private void Respawn() {
        _spr.enabled = true;
        _rbody.simulated = true;
        _collider.enabled = true;
        _timeLastRespawned = Time.time;
        _playerInfo.health = _playerInfo.GetMaxHealth();
        GameHUDScript.UpdatePlayerHealthVisual(_playerNumber);
    }

    private void GameOver() {
        SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "GameOverScene");
    }

}
