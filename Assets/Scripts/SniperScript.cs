using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScript : MonoBehaviour
{
    Rigidbody2D _rbody;
    SpriteRenderer _sRender;
    Transform _transform;

    float _movementSpeed = 5;
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
        if (_rbody.velocity != Vector2.zero)
        {
            _transform.up = _rbody.velocity;
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _rbody.velocity = (new Vector2(x, y) * _movementSpeed);
    }
}
