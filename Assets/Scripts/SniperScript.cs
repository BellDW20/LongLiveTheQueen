using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScript : MonoBehaviour
{
    Rigidbody2D _rbody;
    SpriteRenderer _sRender;
    Transform _transform;

    public GameObject _bullet;

    Vector3 _currentDirection;
    float _movementSpeed = 5;
    float _fireDelay = 0.5f;
    float _shotVelocity = 10;
    float _shotTimer = 0.0f;
    int _magSize = 6;

    float _maxLife = 10;

    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _sRender = GetComponent<SpriteRenderer>();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _currentDirection = _rbody.velocity.normalized;
        if (_rbody.velocity != Vector2.zero)
        {
            _transform.up = _rbody.velocity;
        }

        if (Input.GetMouseButton(0) && Time.time - _shotTimer > _fireDelay)
        {
            Vector3 tempDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);

            Vector3 shotDirection = new Vector3(tempDirection.x - _transform.position.x, tempDirection.y - _transform.position.y, 0);

            _transform.up = shotDirection;
            tempBullet.transform.up = shotDirection;

            tempBullet.GetComponent<Rigidbody2D>().velocity = shotDirection.normalized * _shotVelocity;
            _shotTimer = Time.time;
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _rbody.velocity = (new Vector2(x, y) * _movementSpeed);
    }
}
