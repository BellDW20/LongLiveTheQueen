using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectileScript : MonoBehaviour
{
    [SerializeField] protected float DAMAGE;
    protected int _playerCreatedBy;

    public virtual void OnEnemyHit(GameObject enemy) {
        EnemyHealthScript.DamageAndScore(enemy, DAMAGE, _playerCreatedBy);
        SoundManager.PlaySFX(SFX.ENEMY_HIT);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            OnEnemyHit(other.gameObject);
        }
    }
    
    public void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void SetPlayerCreatedBy(int _playerCreatedBy) {
        this._playerCreatedBy = _playerCreatedBy;
    }

    public int GetPlayerCreatedBy()
    {
        return _playerCreatedBy;
    }

}
