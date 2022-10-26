using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoScript : MonoBehaviour
{

    private Transform _transform;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Gun _gun;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _specialBullet;

    private float _shotVelocity = 10;
    private float _specialShotVelocity = 5;
    private float _specialCooldown = 5.0f;
    private float _specialTimer = -30.0f;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _transform = transform;
        _gun.Init();
        MSMScript.RegisterPlayer(gameObject);
    }

    void Update() {
        _playerController.Update();
        if(_playerController.IsDead()) { return; }

        if(InputManager.GetFireInput(_playerController.GetPlayerNumber())) {
            if(_gun.CanShoot()) {
                GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);

                float shotAngle = Mathf.Atan2(
                    _playerController.GetLookDirection().y, 
                    _playerController.GetLookDirection().x
                ) + _gun.GetSpreadAngle();

                Vector2 shotDirection = new Vector2(Mathf.Cos(shotAngle), Mathf.Sin(shotAngle));

                tempBullet.transform.up = shotDirection;
                tempBullet.GetComponent<Rigidbody2D>().velocity = 10*shotDirection;
                tempBullet.GetComponent<PlayerProjectileScript>().SetPlayerCreatedBy(_playerController.GetPlayerNumber());
            }
        }

        if (InputManager.GetSpecialInput(_playerController.GetPlayerNumber()) && Time.time - _specialTimer >= _specialCooldown)
        {
            GameObject tempSpecial = Instantiate(_specialBullet, _transform.position, Quaternion.identity);
            tempSpecial.transform.up = _playerController.GetLookDirection();
            tempSpecial.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * _specialShotVelocity;
            _specialTimer = Time.time;
        }
    }
}
