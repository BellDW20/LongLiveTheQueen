using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BombExplosionScript : MonoBehaviour {

    private const int DAMAGE = 8;

    public void DestroyObject( ) {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.Damage(DAMAGE);
        }
    }

}
