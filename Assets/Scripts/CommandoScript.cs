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

                float shotAngle = Mathf.Atan2(
                    _playerController.GetLookDirection().y, 
                    _playerController.GetLookDirection().x
                ) + _gun.GetSpreadAngle();

                Vector2 shotDirection = new Vector2(Mathf.Cos(shotAngle), Mathf.Sin(shotAngle));

                tempBullet.transform.up = shotDirection;
                tempBullet.GetComponent<Rigidbody2D>().velocity = 10*shotDirection;
            }
        }
    }

}
