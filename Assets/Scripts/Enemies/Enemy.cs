using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour {

    public float SPEED;
    public float RANGE;
    public float DAMAGE;
    private NavMeshAgent _agent;
    private Rigidbody2D _rbody;
    private Transform _transform;
    private Animations _animations;
    private float _lastPathRefresh;

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _rbody = GetComponent<Rigidbody2D>();
        _animations = new Animations(GetComponent<Animator>(), "Walk");

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = SPEED;

        _transform = transform;
        _lastPathRefresh = Time.time;
    }

    void Update() {
        if (Time.time - _lastPathRefresh > 0.3f) {
            Vector2 dest = MSMScript.NearestPlayerPosition(gameObject);
            _agent.enabled = (_rbody.position - dest).magnitude < RANGE;
            if(_agent.enabled) {
                _agent.SetDestination(dest);
            }
            _lastPathRefresh = Time.time;
        }

        if (_agent.velocity.magnitude > 0.1f) {
            float angle = 180 * Mathf.Atan2(_agent.velocity.y, _agent.velocity.x) / Mathf.PI;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (!pc.IsDead()) { pc.Damage(DAMAGE); }
        }
    }

    

}
