using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private Transform _transform; //Focus position of the camera
    
    //flags indicating whether the camera is allowed to move in each direction
    private bool _canMoveLeft = false;
    private bool _canMoveRight = false;
    private bool _canMoveUp = false;
    private bool _canMoveDown = false;

    private Vector2 _target; //Where the camera intends to move towards

    //The target positions components used in calculation of the _target vector
    private float _rightTargetX, _upTargetY;
    private float _leftTargetX, _downTargetY;

    void Start()
    {
        _transform = transform;
    }

    private void FixedUpdate() {
        //Set the new x & y positions of the camera so that
        //It moves 1/10th of the way from where it is now to the target position
        float nx = _transform.position.x + (_target.x - _transform.position.x) * 0.1f;
        float ny = _transform.position.y + (_target.y - _transform.position.y) * 0.1f;

        //Set the position of the camera to this new smoothly panned position
        _transform.position = new Vector3(nx, ny, _transform.position.z);
    }

    //We use late update to sync camera panning with finalized player movement
    private void LateUpdate()
    {
        //Assume initially that we can move the camera in any direction we'd like
        _canMoveLeft = _canMoveRight = _canMoveUp = _canMoveDown = true;

        //These values keep track of the closest x & y positions
        //to the...
        float bestPDX = float.PositiveInfinity; //left,
        float bestPDY = float.PositiveInfinity; //above,
        float bestNDX = float.NegativeInfinity; //right,
        float bestNDY = float.NegativeInfinity; //and below
        //the camera.

        //For every player's position
        foreach (GameObject player in MSMScript.GetPlayers())
        {
            if(player.GetComponent<PlayerController>().IsDead()) { continue; }
            Transform playerTransform = player.transform;

            //calculate their signed x & y distance to the camera
            float dx = playerTransform.position.x - _transform.position.x;
            float dy = playerTransform.position.y - _transform.position.y;

            //If the players are in the same x-half of the screen (both in left half or both in right half)
            //allow the camera to move in that direction. Do the same for the y-halves. This makes it so that
            //the camera can only move in a direction both players are intending to move in simultaneously
            _canMoveRight &= (dx > 0);
            _canMoveLeft &= (dx < 0);
            _canMoveUp &= (dy > 0);
            _canMoveDown &= (dy < 0);
            
            //Keep track of which x position was closest to the left/right of the camera
            if(dx > 0 && dx < bestPDX) {
                bestPDX = dx;
                _rightTargetX = playerTransform.position.x;
            } else if(dx < 0 && dx > bestNDX) {
                bestNDX = dx;
                _leftTargetX = playerTransform.position.x;
            }

            //Keep track of which y position was closest to the top/bottom of the camera
            if (dy > 0 && dy < bestPDY) {
                bestPDY = dy;
                _upTargetY = playerTransform.position.y;
            }
            else if (dy < 0 && dy > bestNDY) {
                bestNDY = dy;
                _downTargetY = playerTransform.position.y;
            }
        }

        //If all players are in any of the same x or y halves of the screen...
        if(_canMoveRight || _canMoveLeft || _canMoveUp || _canMoveDown) {
            //Assume initially the camera will stay at its current position
            float targetX = _transform.position.x;
            float targetY = _transform.position.y;

            //set the x target position depending on the direction of movement (if possible)
            if (_canMoveRight) {
                targetX = _rightTargetX;
            } else if(_canMoveLeft) {
                targetX = _leftTargetX;
            }

            //set the y target position depending on the direction of movement (if possible)
            if (_canMoveUp) {
                targetY = _upTargetY;
            }
            else if (_canMoveDown) {
                targetY = _downTargetY;
            }

            //Update the target position
            _target = new Vector2(targetX, targetY);
        }

    }
}
