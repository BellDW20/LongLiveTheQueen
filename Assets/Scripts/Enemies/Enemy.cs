using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

//For pathfinding enemies using navmeshes
public class Enemy : MonoBehaviour {

    public float SPEED; //The speed at which this enemy moves
    public float RANGE; //The furthest the targeted player can be before
                        //the enemy no longer pursues the player
    public float DAMAGE; //The amount of damage this enemy deals to players
    private NavMeshAgent _agent;
    private Rigidbody2D _rbody;
    private Transform _transform;
    private Animations _animations;
    private float _lastPathRefresh; //Last time the enemy calculated which player to target

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _rbody = GetComponent<Rigidbody2D>();
        _animations = new Animations(GetComponent<Animator>(), "Walk");

        _agent.updateRotation = false; //Prevents navmesh agent from rotating in 3 dimensions (!!)
        _agent.updateUpAxis = false; // Prevents navmesh agent from thinking up is in a 3rd dimension (!!)
        _agent.speed = SPEED;

        _transform = transform;

        //random time added makes it so that not every agent updates their path on the same frame
        _lastPathRefresh = Time.time+Random.Range(0,1f);
    }

    void Update() {
        //If enough time has passed between when we last found
        //the player we wanted to target...
        if (Time.time - _lastPathRefresh > 0.3f) {
            //Set our destination to the position of the nearest player
            Vector2 dest = MSMScript.NearestPlayerPosition(gameObject);

            //If the nearest player is close enough to us, enable pathfinding.
            //Otherwise, disable it.
            _agent.enabled = (_rbody.position - dest).magnitude < RANGE;

            //If pathfinding is enabled, set the destination to the nearest player's position
            if(_agent.enabled) {
                _agent.SetDestination(dest);
            }

            //reset the last time we updated our pathfinding to now
            _lastPathRefresh = Time.time;
        }

        //If the agent is moving, rotate it to face the direction its moving
        if (_agent.velocity.magnitude > 0.1f) {
            float angle = 180 * Mathf.Atan2(_agent.velocity.y, _agent.velocity.x) / Mathf.PI;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        //If we are still touching a player...
        if(collision.CompareTag("Player")) {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            //As long as the player hasn't died, try to damage them
            //NOTE: this does not hurt the player each frame, as the player Damage() function
            //      takes into account whether or not the player is invulnerable (has been damaged recently)
            if (!pc.IsDead()) { pc.Damage(DAMAGE); }
        }
    }

    

}
