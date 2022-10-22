using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CommandoScript : MonoBehaviour
{

    private Rigidbody2D _rbody;
    private Transform _transform;
    private Animations _animations;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Gun _gun;
    public GameObject _bullet;

    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _animations = new Animations(GetComponent<Animator>(), "Stand");
        _playerController.init(_rbody, _transform, _animations);

        MSMScript.RegisterPlayer(gameObject);
    }

    void Update()
    {

        _playerController.Update();

        if(Input.GetMouseButton(0)) {
            if(_gun.CanShoot()) {
                GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);
                tempBullet.transform.up = _playerController.GetLookDirection();
                tempBullet.GetComponent<Rigidbody2D>().velocity = _playerController.GetLookDirection().normalized * 10;
            }
        }
    }

}
