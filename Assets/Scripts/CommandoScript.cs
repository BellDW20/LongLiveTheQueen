using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoScript : MonoBehaviour
{

    private Transform _transform;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Gun _gun;
    [SerializeField] private GameObject _bullet; //Bullet prefab
    [SerializeField] private GameObject _specialBullet; //Special prefab

    private float _shotVelocity = 10; //Speed of bullets out of the gun
    private float _specialShotVelocity = 5; //Speed of Commando special (molotov)
    private float _specialCooldown = 5.0f; //Cooldown for special
    private float _specialTimer = -30.0f; //Special timer, start negative to make sure it can be used immediately

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _transform = transform;
        _gun.Init();
        MSMScript.RegisterPlayer(gameObject);
    }

    void Update() {
        //Update player controller to make sure he isn't dead
        _playerController.Update();
        if(_playerController.IsDead()) { return; }

        if(InputManager.GetFireInput(_playerController.GetPlayerNumber())) {
            //Check if gun can shoot to be sure player isn't reloading or on a cooldown
            if(_gun.CanShoot()) {

                //Play shot sound effect and create bullet at player's location
                SoundManager.PlaySFX(SoundManager.SFX_MACHINE_GUN_SHOT);
                GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);

                //Determine shot angle from where the player is looking 
                float shotAngle = Mathf.Atan2(
                    _playerController.GetLookDirection().y, 
                    _playerController.GetLookDirection().x
                ) + _gun.GetSpreadAngle(); //Add spread (only applies to commando)

                //Determine shot direction
                Vector2 shotDirection = new Vector2(Mathf.Cos(shotAngle), Mathf.Sin(shotAngle));

                //Make sure that the bullet is facing in the direction it gets shot
                tempBullet.transform.up = shotDirection;
                //Set velocity and the player who created the shot
                tempBullet.GetComponent<Rigidbody2D>().velocity = _shotVelocity*shotDirection;
                tempBullet.GetComponent<PlayerProjectileScript>().SetPlayerCreatedBy(_playerController.GetPlayerNumber());
            }
        } else if (InputManager.GetReloadInput(_playerController.GetPlayerNumber())) {
            _gun.Reload();
        }

        //Updates amount of ammo player has
        GameHUDScript.UpdatePlayerAmmoVisual(_playerController.GetPlayerNumber(), _gun);

        //Check if player is shooting special and if the special cooldown is ready
        if (InputManager.GetSpecialInput(_playerController.GetPlayerNumber()) && Time.time - _specialTimer >= _specialCooldown)
        {
            //Instantiate special prefab
            GameObject tempSpecial = Instantiate(_specialBullet, _transform.position, Quaternion.identity);
            //Make sure that special is facing in the direction it gets shot
            tempSpecial.transform.up = _playerController.GetLookDirection();
            //Set shot velocity
            tempSpecial.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * _specialShotVelocity;
            _specialTimer = Time.time;
        }

        //Updates the "Special Ready" text
        GameHUDScript.UpdatePlayerSpecialVisual(_playerController.GetPlayerNumber(), Time.time - _specialTimer >= _specialCooldown);
    }
}
