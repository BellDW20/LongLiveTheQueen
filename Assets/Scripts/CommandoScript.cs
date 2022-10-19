using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoScript : MonoBehaviour
{
    Rigidbody2D _rbody;
    SpriteRenderer _sRender;
    Transform _transform;

    float _movementSpeed = 5;
    const float DASH_SPEED = 10;
    const float DASH_TIME = 0.1f;
    bool _isDashing = false;
    float _dashStart;
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
    }

    private void FixedUpdate()
    {
        if (_isDashing) return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _rbody.velocity = (new Vector2(x, y) * _movementSpeed);
    }
}
