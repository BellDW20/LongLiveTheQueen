using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour {

    [SerializeField] private float max_health; //The maximum health of the enemy
    [SerializeField] private bool isBoss; //Whether or not this enemy is a boss
    [SerializeField] private GameObject BLOOD_PARTICLES; //Blood particle prefab to spawn when damaged
    [SerializeField] private GameObject DAMAGE_PARTICLE; //Damage amount particle prefab to spawn when damaged
    private Transform _transform; //Position of the enemy
    private float health; //Current health of the enemy
    private float damageTaken; //The amount of damage this enemy has taken over time

    public void Start() {
        if(isBoss) {
            NewGameHUD.BeginBossFight(this);
        }
        //Set our health to max
        health = max_health;
        _transform = transform;
    }

    public void ScaleHealth(float scale) {
        health *= scale;
        max_health *= scale;
    }

    public void Heal(float hp) {
        //Adjust the health by the passed value, capping health to max_health
        health = Mathf.Min(health + hp, 2*max_health);
    }

    //Damages the enemy and returns the score assigned to the damage value
    public float Damage(float hp) {
        //Remove the passed health value, and spawn a blood particle at the enemy's position
        float prevHealth = health;
        health -= hp;

        //If the enemy has no health...
        if (health <= 0) {
            health = 0;
            //Play a death sound effect and destroy the enemy object
            SoundManager.PlaySFX(SFX.ENEMY_DEATH);
            Destroy(gameObject);
        }

        damageTaken += (prevHealth - health);

        //Create particles (blood and number indicating damage dealt)
        if (Random.value > 0.7f) {
            Instantiate(BLOOD_PARTICLES, _transform.position, Quaternion.identity);
        }

        GameObject dmgParticle = Instantiate(DAMAGE_PARTICLE, _transform.position, Quaternion.identity);
        dmgParticle.GetComponent<DamageIndicatorScript>().SetText("" + (int)(prevHealth - health));

        //return the total 
        return (damageTaken < 1.1f*max_health) ? (prevHealth - health) : 0;
    }

    public float GetHealth() {
        return health;
    }

    public float GetMaxHealth() {
        return max_health;
    }

    //Used to both damage an enemy which has an EnemyHealthScript while taking into account
    //the player's damage boost factor from leveling, and adding that damage to their score.
    public static void DamageAndScore(GameObject enemyToDamage, float rawDamage, int player)
    {
        //Get the health script of the enemy
        EnemyHealthScript enHealth = enemyToDamage.GetComponent<EnemyHealthScript>();

        //use the player's damage scaling to calculate the actual damage dealt
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];
        float scaledDamage = rawDamage * pInfo.GetDamageScale();

        //Damage the enemy the appropriate amount
        float actualDamage = enHealth.Damage(scaledDamage);

        //Add this damage to the player's score...
        pInfo.AddToScore((int)actualDamage);

        //Update the player's score bar on the HUD
        NewGameHUD.UpdatePlayerScoreVisual(player);
    }

}
