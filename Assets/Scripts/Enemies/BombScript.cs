using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BombScript : MonoBehaviour {

    public GameObject EXPLOSION_ANIM; //The explosion animation to spawn on impact
    public GameObject EXPLOSION_PARTICLES; //The explosion particles to spawn on impact
    public Vector2 target; //The position the bomb intends to detonate at
    public float speed; //The speed at which this bomb is moving

    private Rigidbody2D _rbody;

    void Start() {
        _rbody = GetComponent<Rigidbody2D>();

        //Play a whistling sound as this bomb goes to its target
        SoundManager.PlaySFX(SoundManager.SFX_BOMB_WHISTLE);
    }

    void Update() {
        //Get the non-normalized direction vector to the target
        Vector2 direction = (target - _rbody.position);

        //if we got close enough to our original target, explode
        float dist = direction.magnitude;
        if(dist < 0.1) {
            Explode();
            return;
        }

        //otherwise, move in the direction of the target at the set speed
        direction /= dist;
        _rbody.velocity = speed * direction;
    }

    private void Explode() {
        //When we explode, spawn both the explosion animation and particles...
        Instantiate(EXPLOSION_ANIM, _rbody.position, Quaternion.identity);
        Instantiate(EXPLOSION_PARTICLES, _rbody.position, Quaternion.identity);
        //And get rid of this bomb projectile
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //if we touch a player before reaching the target position, explode
        if(collision.CompareTag("Player")) {
            Explode();
        }
    }

}
