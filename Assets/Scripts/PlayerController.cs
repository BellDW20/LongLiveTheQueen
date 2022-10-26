using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    private static readonly Color INVULN_COLOR = new Color(1, 0.5f, 0.5f, 0.75f);

    public float MOVE_SPEED;
    public float DASH_SPEED;
    public float DASH_TIME;
    public float INVULN_TIME;
    
    private bool _isDashing = false, _initComplete = false;
    private float _dashStartTime;
    private float _timeLastDamaged;

    private Rigidbody2D _rbody;
    private Transform _transform;
    private Animations _animations;
    private SpriteRenderer _spr;
    private int _playerNumber;
    private PlayerInfo _playerInfo;
    private Camera _cam;

    private Vector2 _lookDirection;

    public void Init() {
        _rbody = GetComponent<Rigidbody2D>();
        _animations = new Animations(GetComponent<Animator>(), "Stand");
        _spr = GetComponent<SpriteRenderer>();
        _transform = transform;
        _playerNumber = LevelManagerScript.GetPlayerNumber(gameObject);
        _playerInfo = LevelManagerScript.pInfos[_playerNumber];
        _cam = Camera.main;
        _initComplete = true;
    }

    public void Update() {
        if(!_initComplete) { Init(); }

        //Temp camera code
        _cam.transform.position = new Vector3(_transform.position.x, _transform.position.y, _cam.transform.position.z);

        if(IsInvulnerable()) {
            _spr.color = INVULN_COLOR;
        } else {
            _spr.color = Color.white;
        }

        if (InputManager.GetDashInput(_playerNumber))
        {
            _rbody.velocity = _rbody.velocity.normalized*DASH_SPEED;
            _isDashing = true;
            _dashStartTime = Time.time;
        }

        if (Time.time - _dashStartTime >= DASH_TIME) {
            _rbody.velocity = (_rbody.velocity.normalized * MOVE_SPEED); ;
            _isDashing = false;
        }

        Vector2 input;
        if (_playerNumber == 0)
        {
            input = new Vector2(InputManager.GetHorizontalKeyboard(), InputManager.GetVerticalKeyboard());
        }
        else
        {
            input = new Vector2(InputManager.GetHorizontalGamepad(_playerNumber), InputManager.GetVerticalGamepad(_playerNumber));
        }

        if (_rbody.velocity.magnitude > 0.1f && input.magnitude > 0.1f) {
            _animations.SetAnimation("Walk");
        } else {
            _animations.SetAnimation("Stand");
        }

        _lookDirection = ChangeLookDirection();
        UpdateLookDirection();

        if (_isDashing) return;

        _rbody.velocity = (input * MOVE_SPEED);
    }

    private Vector2 ChangeLookDirection()
    {
        if (_playerNumber == 0)
        {
            Vector3 tempDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
            return new Vector2(tempDir.x, tempDir.y);

        }
        else
        {
            return new Vector2(InputManager.GetAimInputGamepadHorizontal(_playerNumber), InputManager.GetAimInputGamepadVertical(_playerNumber));
        }
    }

    private void UpdateLookDirection() {
        //update rotation to follow looking direction
        _transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, 180 * Mathf.Atan2(_lookDirection.y, _lookDirection.x) / Mathf.PI - 90)
        );
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
        return (Time.time - _timeLastDamaged) < INVULN_TIME;
    }

    public void Damage(float amt) {
        if(!IsInvulnerable()) {
            _playerInfo.health -= amt;
            _timeLastDamaged = Time.time;
            GameHUDScript.UpdatePlayerHealthVisual(_playerNumber);
        }
    }

}
