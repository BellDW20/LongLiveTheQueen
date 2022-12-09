using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroBulletScript : PlayerProjectileScript {

    // Start is called before the first frame update
    void Start() {
        Invoke("Die", 0.05f);
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        EnemyHealthScript.DamageAndScore(enemy, DAMAGE, _playerCreatedBy);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
