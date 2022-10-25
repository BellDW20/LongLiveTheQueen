using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerProjectileScript : MonoBehaviour
{

    [SerializeField] private float DAMAGE;
    private int _playerCreatedBy;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            EnemyHealthScript health = other.gameObject.GetComponent<EnemyHealthScript>();
            //update score of player based on projectile's player creation number
            health.Damage(DAMAGE);
        }
        Destroy(gameObject);
    }
    
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void SetPlayerCreatedBy(int _playerCreatedBy) {
        this._playerCreatedBy = _playerCreatedBy;
    }

}
