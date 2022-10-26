using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BombScript : MonoBehaviour {

    public GameObject EXPLOSION_ANIM;
    public GameObject EXPLOSION_PARTICLES;
    public Vector2 target;
    public float speed;

    private Rigidbody2D _rbody;

    void Start() {
        _rbody = GetComponent<Rigidbody2D>();
        SoundManager.PlaySFX(SoundManager.SFX_BOMB_WHISTLE);
    }

    void Update() {
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
        Instantiate(EXPLOSION_ANIM, _rbody.position, Quaternion.identity);
        Instantiate(EXPLOSION_PARTICLES, _rbody.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //if we touch a player prematurely, explode
        if(collision.CompareTag("Player")) {
            Explode();
        }
    }

}
