using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerProjectileScript : MonoBehaviour
{

    [SerializeField] private float DAMAGE;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            EnemyHealthScript health = other.gameObject.GetComponent<EnemyHealthScript>();
            health.Damage(DAMAGE);
        }
        Destroy(gameObject);
    }
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

}
