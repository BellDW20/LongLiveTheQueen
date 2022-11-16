using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageScript : MonoBehaviour {

    private float GROUP_DETECTION_RADIUS = 4;
    private float GROUP_UPDATE_TIME = 0.2f;
    private float MOVE_SPEED = 3;

    private float _lastGroupUpdateTime;
    private Transform _transform;
    private Vector3 _vel, _lastVel, _actualVel;
    [SerializeField] private GameObject _healParticles;
    [SerializeField] private LayerMask _enemyMask;

    void Start() {
        _transform = transform;
        _lastGroupUpdateTime = Time.time; 
        _vel = Vector3.zero;
        _lastVel = Vector3.zero;
    }

    void Update() {
        float dt = Time.time - _lastGroupUpdateTime;
        if (dt > GROUP_UPDATE_TIME) {
            FollowLocalGroup();
            _lastGroupUpdateTime = Time.time;
        } else {
            _transform.position += _actualVel * Time.deltaTime;
        }
    }

    private void FollowLocalGroup() {
        Collider2D[] hit = Physics2D.OverlapCircleAll(
            new Vector2(_transform.position.x, _transform.position.y),
            GROUP_DETECTION_RADIUS,
            _enemyMask
        );
        if(hit == null) { return; }

        int nonMageCount = 0;
        Vector3 targetPos = Vector3.zero;
        Vector3 posn;
        for (int i=0; i<hit.Length; i++) {
            if(hit[i].CompareTag("Mage")) { continue; }
            posn = hit[i].transform.position;
            targetPos += posn;
            hit[i].GetComponent<EnemyHealthScript>().Heal(5);
            if (Random.value < 0.2f) {
                Instantiate(_healParticles, posn, Quaternion.identity);
            }
            nonMageCount++;
        }
        
        if(nonMageCount == 0) {
            targetPos = MSMScript.NearestPlayerPosition(gameObject);
        } else {
            targetPos *= (1.0f / nonMageCount);
        }

        _lastVel = _vel;
        _vel = (targetPos - _transform.position).normalized;
        _actualVel = (0.9f * MOVE_SPEED) * _vel + (0.1f * MOVE_SPEED) * _lastVel;

        float theta = 180*Mathf.Atan2(_actualVel.y, _actualVel.x)/Mathf.PI-90;
        _transform.rotation = Quaternion.Euler(0, 0, theta);
    }

}
