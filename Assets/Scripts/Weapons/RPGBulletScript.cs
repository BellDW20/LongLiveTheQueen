using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBulletScript : PlayerProjectileScript
{
    Rigidbody2D _rbody;
    public GameObject EXPLOSION_ANIM; //The explosion animation to spawn on impact
    public GameObject EXPLOSION_PARTICLES; //The explosion particles to spawn on impact
    const float _EXPLOSION_RANGE = 2;

    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        EnemyHealthScript.DamageAndScore(enemy, DAMAGE, _playerCreatedBy);
        SoundManager.PlaySFX(SFX.ENEMY_HIT);
        Explode();
    }

    void Explode()
    {
        //Instantiate animation and particle effect
        Instantiate(EXPLOSION_ANIM, _rbody.position, Quaternion.identity);
        Instantiate(EXPLOSION_PARTICLES, _rbody.position, Quaternion.identity);

        List<Collider2D> enemies = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        //Get objects in circular range specified
        int numCollisions = Physics2D.OverlapCircle(_rbody.position, _EXPLOSION_RANGE, filter.NoFilter(), enemies);

        if (numCollisions > 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                //For each collision, if it is an enemy, damage the enemy and add to player's score
                GameObject enemyGameObject = enemy.gameObject;
                if (enemyGameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    EnemyHealthScript.DamageAndScore(enemyGameObject, 25, _playerCreatedBy);
                }
            }
        }

        Destroy(gameObject);
    }
}
