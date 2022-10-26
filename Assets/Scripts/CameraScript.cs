using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    Transform _transform;
    List<Transform> _playerTransforms = new List<Transform>();
    Vector3 _offset = new Vector3(2, 0, -10);
    private float speed = 10.0f;
    [SerializeField] private MSMScript _msManager;
    
    private bool _canMoveLeft = false;
    private bool _canMoveRight = false;
    private bool _canMoveUp = false;
    private bool _canMoveDown = false;

    private Vector2 _target;
    private float _rightTargetX, _upTargetY;
    private float _leftTargetX, _downTargetY;

    void Start()
    {
        _transform = transform;
    }

    private void FixedUpdate() {
        float nx = _transform.position.x + (_target.x - _transform.position.x) * 0.1f;
        float ny = _transform.position.y + (_target.y - _transform.position.y) * 0.1f;
        _transform.position = new Vector3(nx, ny, _transform.position.z);
    }

    private void LateUpdate()
    {
        if(_playerTransforms.Count < 1)
        {
            foreach (GameObject player in _msManager.GetPlayers())
            {
                _playerTransforms.Add(player.transform);
            }
        }

        _canMoveLeft = _canMoveRight = _canMoveUp = _canMoveDown = true;

        float bestPDX = float.PositiveInfinity;
        float bestPDY = float.PositiveInfinity;
        float bestNDX = float.NegativeInfinity;
        float bestNDY = float.NegativeInfinity;

        foreach (Transform playerTransform in _playerTransforms)
        {
            float dx = playerTransform.position.x - _transform.position.x;
            float dy = playerTransform.position.y - _transform.position.y;

            _canMoveRight &= (dx > 0);
            _canMoveLeft &= (dx < 0);
            _canMoveUp &= (dy > 0);
            _canMoveDown &= (dy < 0);
            
            if(dx > 0 && dx < bestPDX) {
                bestPDX = dx;
                _rightTargetX = playerTransform.position.x;
            } else if(dx < 0 && dx > bestNDX) {
                bestNDX = dx;
                _leftTargetX = playerTransform.position.x;
            }

            if (dy > 0 && dy < bestPDY) {
                bestPDY = dy;
                _upTargetY = playerTransform.position.y;
            }
            else if (dy < 0 && dy > bestNDY) {
                bestNDY = dy;
                _downTargetY = playerTransform.position.y;
            }
        }

        if(_canMoveRight || _canMoveLeft || _canMoveUp || _canMoveDown) {
            float targetX = _transform.position.x;
            float targetY = _transform.position.y;
            if (_canMoveRight) {
                targetX = _rightTargetX;
            } else if(_canMoveLeft) {
                targetX = _leftTargetX;
            }

            if (_canMoveUp) {
                targetY = _upTargetY;
            }
            else if (_canMoveDown) {
                targetY = _downTargetY;
            }
            _target = new Vector2(targetX, targetY);
        }

    }
}
