using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour {

    [SerializeField] private float max_health;
    [SerializeField] private GameObject BLOOD_PARTICLES;
    private Transform _transform;
    private float health;

    public void Start() {
        health = max_health;
        _transform = transform;
    }

    public void Heal(float hp) {
        health = Mathf.Min(health + hp, max_health);
    }

    public void Damage(float hp) {
        health -= hp;
        Instantiate(BLOOD_PARTICLES, _transform.position, Quaternion.identity);
        if (health <= 0) {
            SoundManager.PlaySFX(SoundManager.SFX_ENEMY_DEATH);
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
        if(pInfo.AddToScore((int)actualDamage)) {
            GameHUDScript.UpdatePlayerLevelVisual(player);
        }
        GameHUDScript.UpdatePlayerScoreVisual(player);
        enHealth.Damage(rawDamage * pInfo.damageScale);
    }

}
