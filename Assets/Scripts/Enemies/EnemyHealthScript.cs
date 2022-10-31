using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour {

    [SerializeField] private float max_health; //The maximum health of the enemy
    [SerializeField] private GameObject BLOOD_PARTICLES; //Blood particle prefab to spawn when damaged
    private Transform _transform; //Position of the enemy
    private float health; //Current health of the enemy

    public void Start() {
        //Set our health to max
        health = max_health;
        _transform = transform;
    }

    public void Heal(float hp) {
        //Adjust the health by the passed value, capping health to max_health
        health = Mathf.Min(health + hp, max_health);
    }

    public void Damage(float hp) {
        //Remove the passed health value, and spawn a blood particle at the enemy's position
        health -= hp;
        Instantiate(BLOOD_PARTICLES, _transform.position, Quaternion.identity);

        //If the enemy has no health...
        if (health <= 0) {
            //Play a death sound effect and destroy the enemy object
            SoundManager.PlaySFX(SoundManager.SFX_ENEMY_DEATH);
            Destroy(gameObject);
        }
    }

    public float GetHealth() {
        return health;
    }

    //Used to both damage an enemy which has an EnemyHealthScript while taking into account
    //the player's damage boost factor from leveling, and adding that damage to their score.
    public static void DamageAndScore(GameObject enemyToDamage, float rawDamage, int player)
    {
        //Get the health script of the enemy
        EnemyHealthScript enHealth = enemyToDamage.GetComponent<EnemyHealthScript>();

        //use the player's damage scaling to calculate the actual damage dealt
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];
        float actualDamage = rawDamage * pInfo.damageScale;

        //Add this damage to the player's score...
        if(pInfo.AddToScore((int)actualDamage)) {
            //And if the player leveled up, update their level in the HUD
            GameHUDScript.UpdatePlayerLevelVisual(player);
        }
        //Update the player's score bar on the HUD
        GameHUDScript.UpdatePlayerScoreVisual(player);

        //Damage the enemy the appropriate amount
        enHealth.Damage(rawDamage * pInfo.damageScale);
    }

}
