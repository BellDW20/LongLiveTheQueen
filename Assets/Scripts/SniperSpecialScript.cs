using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SniperSpecialScript : PlayerProjectileScript {
    public override void OnEnemyHit(GameObject enemy)
    {
        EnemyHealthScript.DamageAndScore(enemy, DAMAGE, _playerCreatedBy);
        SoundManager.PlaySFX(SFX.ENEMY_HIT);
    }
}
