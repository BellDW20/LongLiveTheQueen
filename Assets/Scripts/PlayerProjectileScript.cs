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
            EnemyHealthScript.DamageAndScore(other.gameObject, DAMAGE, _playerCreatedBy);
            SoundManager.PlaySFX(SoundManager.SFX_ENEMY_HIT);
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
