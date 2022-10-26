using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour {

    [SerializeField] private float max_health;
    private float health;

    public void Start() {
        health = max_health;
    }

    public void Heal(float hp) {
        health = Mathf.Min(health + hp, max_health);
    }

    public void Damage(float hp) {
        health -= hp;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public float GetHealth() {
        return health;
    }

    public static void DamageAndScore(GameObject enemyToDamage, float rawDamage, int player)
    {
        EnemyHealthScript enHealth = enemyToDamage.GetComponent<EnemyHealthScript>();
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];
        float actualDamage = rawDamage * pInfo.damageScale;
        pInfo.AddToScore((int)actualDamage);
        GameHUDScript.UpdatePlayerScoreVisual(player);
        enHealth.Damage(rawDamage * pInfo.damageScale);
    }

}
