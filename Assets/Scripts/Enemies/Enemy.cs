using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour {

    public float SPEED;
    public float START_HEALTH;

    private float health;
    private NavMeshAgent _agent;
    private Rigidbody2D _rbody;
    private Transform _transform;
    
    private Animator _animator;
    private string currentAnimation;

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _rbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = SPEED;

        currentAnimation = "Walk";
        _animator.Play(currentAnimation);

        _transform = transform;

        health = START_HEALTH;
    }

    void Update() {
        _agent.SetDestination(NavManager.nearestPlayerPosition(_transform.position));

        float angle = 180*Mathf.Atan2(_agent.velocity.y, _agent.velocity.x)/Mathf.PI;

        _transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }

    public void damage(float dmg) {
        health -= dmg;
        if(health <= 0) {
            Destroy(this);
        }
    }

    private void setAnimation(string animation) {
        if (currentAnimation.Equals(animation)) {
            return;
        }
        currentAnimation = animation;
        _animator.Play(animation);
    }

}
