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

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if(_playerTransforms.Count < 1)
        {
            foreach (GameObject player in _msManager.GetPlayers())
            {
                print(player.name);
                _playerTransforms.Add(player.transform);
            }
        }
        float playerTotalX = 0;
        float playerTotalY = 0;

        _canMoveLeft = _canMoveRight = _canMoveUp = _canMoveDown = false;

        foreach (Transform playerTransform in _playerTransforms)
        {
            if(playerTransform.position.x > _transform.position.x)
            {
                _canMoveRight = true;
            }
            else if(playerTransform.position.x < _transform.position.x)
            {
                _canMoveLeft = true;
            }

            if (playerTransform.position.y > _transform.position.y)
            {
                _canMoveUp = true;
            }
            else if (playerTransform.position.y < _transform.position.y)
            {
                _canMoveDown = true;
            }
            playerTotalX += playerTransform.position.x;
            playerTotalY += playerTransform.position.y;
        }

        float moveTowardX = _transform.position.x;
        if (_canMoveLeft ^ _canMoveRight)
        {
            moveTowardX = playerTotalX / _playerTransforms.Count;
        }

        float moveTowardY = _transform.position.y;
        if(_canMoveUp ^ _canMoveDown)
        {
            moveTowardY = playerTotalY / _playerTransforms.Count;
        }
        //print(moveTowardX);

        //float step = speed * Time.deltaTime;
        //_transform.position = Vector3.MoveTowards(_transform.position, new Vector3(moveTowardX, moveTowardY, _transform.position.z), step);
        _transform.position = new Vector3(moveTowardX, moveTowardY, _transform.position.z);
    }
}
