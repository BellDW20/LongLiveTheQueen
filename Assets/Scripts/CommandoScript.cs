using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoScript : MonoBehaviour
{

    private Transform _transform;
    private PlayerController _playerController;
    [SerializeField] private Gun _gun;
    [SerializeField] private GameObject _bullet;

    void Start() {
        _playerController = GetComponent<PlayerController>();
        _transform = transform;
        _gun.Init();
        MSMScript.RegisterPlayer(gameObject);
    }

    void Update() {
        _playerController.Update();

        if(Input.GetMouseButton(0)) {
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
    }

}
