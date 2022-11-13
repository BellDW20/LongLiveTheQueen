using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BombExplosionScript : MonoBehaviour {

    private const int DAMAGE = 8; //Amount of damage to deal to players hit by the explosion

    public void Start() {
        //Play an explosion sound
        SoundManager.PlaySFX(SFX.EXPLOSION);
    }

    public void DestroyObject() {
        //Triggered when the animation of the explosion finishes,
        //Destroying this explosion when it's animation is done.
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        //If we come in contact with a player
        if(collision.CompareTag("Player")) {
            //Damage that player the appropriate amount
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.Damage(DAMAGE);
        }
    }

}
