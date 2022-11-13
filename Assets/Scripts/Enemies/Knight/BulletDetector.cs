using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BulletDetector : MonoBehaviour {

    private Vector2 _lastDetectedBulletDirection;
    private bool _isBulletDetected = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Bullet") && !_isBulletDetected) {
            Vector2 _bVel = collision.GetComponent<Rigidbody2D>().velocity;
            _lastDetectedBulletDirection = new Vector2(_bVel.x, _bVel.y);
            _isBulletDetected = true;
        }
    }

    public Vector2 ReadLastDetectedBulletDirection() {
        return _lastDetectedBulletDirection;
    }

    public void ResetDetector() {
        _isBulletDetected=false;
    }

    public bool IsBulletDetected() {
        return _isBulletDetected;
    }

}
