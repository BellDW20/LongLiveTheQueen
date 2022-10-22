using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController {

    public float MOVE_SPEED;
    public float DASH_SPEED;
    public float DASH_TIME;
    
    private bool _isDashing = false;
    private float _dashStartTime;

    private Rigidbody2D _rbody;
    private Transform _transform;
    private Animations _animations;

    private Vector2 _lookDirection;

    public void init(Rigidbody2D _rbody, Transform _transform, Animations _animations) {
        this._rbody = _rbody;
        this._transform = _transform;
        this._animations = _animations;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            _rbody.velocity *= DASH_SPEED;
            _isDashing = true;
            _dashStartTime = Time.time;
        }

        if (Time.time - _dashStartTime >= DASH_TIME) {
            _rbody.velocity = (new Vector2(_rbody.velocity.normalized.x, _rbody.velocity.normalized.y) * MOVE_SPEED); ;
            _isDashing = false;
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_rbody.velocity.magnitude > 0.1f && input.magnitude > 0.1f) {
            _animations.SetAnimation("Walk");
        } else {
            _animations.SetAnimation("Stand");
        }

        UpdateLookDirection();

        if (_isDashing) return;

        _rbody.velocity = (input * MOVE_SPEED);
    }

    private void UpdateLookDirection() {
        Vector3 tempDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
        _lookDirection = new Vector2(tempDir.x, tempDir.y);

        //update rotation to follow looking direction
        _transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, 180 * Mathf.Atan2(_lookDirection.y, _lookDirection.x) / Mathf.PI - 90)
        );
    }

    public Vector2 GetLookDirection() {
        return _lookDirection;
    }

}
