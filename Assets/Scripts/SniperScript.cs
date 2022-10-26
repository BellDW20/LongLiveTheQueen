using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScript : MonoBehaviour
{

    private Transform _transform;
    private PlayerController _playerController;
    [SerializeField] public Gun _gun;
    [SerializeField] public GameObject _bullet;
    [SerializeField] public GameObject _specialBullet;

    private float _shotVelocity = 10;
    private float _specialCooldown = 5.0f;
    private float _specialTimer = -30.0f;

    void Start() {
        _playerController = GetComponent<PlayerController>();
        _transform = transform;
        _gun.Init();
        MSMScript.RegisterPlayer(gameObject);
    }

    void Update() {
        _playerController.Update();
        if (_playerController.IsDead()) { return; }

        if (InputManager.GetFireInput(_playerController.GetPlayerNumber())) {
            if (_gun.CanShoot()) {
                GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);
                tempBullet.transform.up = _playerController.GetLookDirection();
                tempBullet.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * _shotVelocity;
                tempBullet.GetComponent<PlayerProjectileScript>().SetPlayerCreatedBy(_playerController.GetPlayerNumber());
            }
        }

        if (InputManager.GetSpecialInput(_playerController.GetPlayerNumber()) && Time.time - _specialTimer >= _specialCooldown)
        {
            GameObject tempSpecial = Instantiate(_specialBullet, _transform.position, Quaternion.identity);
            tempSpecial.transform.up = _playerController.GetLookDirection();
            tempSpecial.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * _shotVelocity;
            tempSpecial.GetComponent<SniperSpecialScript>().SetPlayerCreatedBy(_playerController.GetPlayerNumber());

            SniperSpecialScript script = tempSpecial.GetComponent<SniperSpecialScript>();
            script.SetIsActive();
            script.SetSplits(3);
            _specialTimer = Time.time;
        }
    }

}
