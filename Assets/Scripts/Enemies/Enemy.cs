using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour {

    public float SPEED;
    private NavMeshAgent _agent;
    private Rigidbody2D _rbody;
    private Transform _transform;
    
    private Animations _animations;

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _rbody = GetComponent<Rigidbody2D>();
        _animations = new Animations(GetComponent<Animator>(), "Walk");

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = SPEED;

        _transform = transform;
    }

    void Update() {
        _agent.SetDestination(MSMScript.NearestPlayerPosition(gameObject));

        if (_agent.velocity.magnitude > 0.1f) {
            float angle = 180 * Mathf.Atan2(_agent.velocity.y, _agent.velocity.x) / Mathf.PI;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
    }

}
