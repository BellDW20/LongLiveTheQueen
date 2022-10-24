using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScript : MonoBehaviour
{
    private Rigidbody2D _rbody;
    private Transform _transform;
    private Animations _animations;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] public Gun _gun;
    [SerializeField] public GameObject _bullet;

    private float _shotVelocity = 10;
    private float _maxLife = 10;

    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _animations = new Animations(GetComponent<Animator>(), "Walk");
        _playerController.Init(_rbody, _transform, _animations);
        _gun.Init();

        MSMScript.RegisterPlayer(gameObject);
    }

    void Update()
    {
        _playerController.Update();

        if (Input.GetMouseButton(0)) {
            if (_gun.CanShoot()) {
                GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);
                tempBullet.transform.up = _playerController.GetLookDirection();
                tempBullet.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * _shotVelocity;
            }
        }
    }

}
