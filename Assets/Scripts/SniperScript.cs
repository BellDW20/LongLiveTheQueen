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
    const float DASH_SPEED = 10;
    const float DASH_TIME = 0.1f;
    bool _isDashing = false;
    float _dashStart;

    float _fireDelay = 0.5f;
    float _shotVelocity = 10;
    float _shotTimer = 0.0f;

    int _magSize = 6;
    int _bulletsInMag = 6;
    float _reloadTimer = 0.0f;
    float _reloadDelay = 1;

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

        if (Time.time - _reloadTimer > _reloadDelay && _reloadTimer != 0)
        {
            _reloadTimer = 0.0f;
            _bulletsInMag = 6;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _rbody.velocity *= DASH_SPEED;
            _isDashing = true;
            _dashStart = Time.time;
        }

        if (Time.time - _dashStart >= DASH_TIME)
        {
            _rbody.velocity = (new Vector2(_rbody.velocity.normalized.x, _rbody.velocity.normalized.y) * _movementSpeed); ;
            _isDashing = false;
        }
        if (_rbody.velocity != Vector2.zero)
        {
            _transform.up = _rbody.velocity;
        }

        if (Input.GetMouseButton(0) && Time.time - _shotTimer > _fireDelay && _bulletsInMag > 0)
        {
            Vector3 tempDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject tempBullet = Instantiate(_bullet, _transform.position, Quaternion.identity);

            Vector3 shotDirection = new Vector3(tempDirection.x - _transform.position.x, tempDirection.y - _transform.position.y, 0);

            _transform.up = shotDirection;
            tempBullet.transform.up = shotDirection;

            tempBullet.GetComponent<Rigidbody2D>().velocity = shotDirection.normalized * _shotVelocity;
            _shotTimer = Time.time;

            _bulletsInMag--;

            if (_bulletsInMag == 0)
            {
                _reloadTimer = Time.time;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isDashing) return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _rbody.velocity = (new Vector2(x, y) * _movementSpeed);
    }
}
