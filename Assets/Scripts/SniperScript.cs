using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScript : MonoBehaviour
{

    private Transform _transform;
    private PlayerController _playerController; //Object detailing controls that apply to all characters
    [SerializeField] public Gun _gun;           //Object detailing firing mechanics that apply to all characters
    [SerializeField] public GameObject _bullet; //Bullet prefab
    [SerializeField] public GameObject _specialBullet; //Special attack prefab

    private float _shotVelocity = 10;
    private float _specialCooldown = 5.0f;
    private float _specialTimer = -30.0f; //Timer for checking special attack cooldown

    void Start() {
        _playerController = GetComponent<PlayerController>();
        _transform = transform;
        _gun.Init();
        MSMScript.RegisterPlayer(gameObject); //Registers playerID
    }

    void Update() {
        _playerController.Update(); //Player controller update for movement
        if (_playerController.IsDead()) { return; }

        //Shooting input
        if (InputManager.GetFireInput(_playerController.GetPlayerNumber())) {

            //can shoot takes into account fire rate and reloading
            if (_gun.CanShoot()) {
                SoundManager.PlaySFX(SoundManager.SFX_SNIPER_RIFLE_SHOT);

                //Instantiate bullet, rotate it correctly, and apply velocity
                GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);
                tempBullet.transform.up = _playerController.GetLookDirection();
                tempBullet.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * _shotVelocity;
                tempBullet.GetComponent<PlayerProjectileScript>().SetPlayerCreatedBy(_playerController.GetPlayerNumber());
            }
        } else if(InputManager.GetReloadInput(_playerController.GetPlayerNumber())) {
            _gun.Reload(); //Reloading input
        }
        GameHUDScript.UpdatePlayerAmmoVisual(_playerController.GetPlayerNumber(), _gun);

        //Special attack input
        if (InputManager.GetSpecialInput(_playerController.GetPlayerNumber()) && Time.time - _specialTimer >= _specialCooldown)
        {
            //Instantiate special bullet, adjust rotation, add a velocity
            GameObject tempSpecial = Instantiate(_specialBullet, _transform.position, Quaternion.identity);
            tempSpecial.transform.up = _playerController.GetLookDirection();
            tempSpecial.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * _shotVelocity;
            tempSpecial.GetComponent<SniperSpecialScript>().SetPlayerCreatedBy(_playerController.GetPlayerNumber());

            //Set split count for special shot
            SniperSpecialScript script = tempSpecial.GetComponent<SniperSpecialScript>();
            script.SetIsActive();
            script.SetSplits(3);
            _specialTimer = Time.time;
        }
        //Display if special is off cooldown
        GameHUDScript.UpdatePlayerSpecialVisual(_playerController.GetPlayerNumber(), Time.time - _specialTimer >= _specialCooldown);
    }

}
