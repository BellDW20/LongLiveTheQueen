using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoSpecialScript : PlayerProjectileScript
{
    public GameObject EXPLOSION_ANIM; //Explosion animation
    public GameObject EXPLOSION_PARTICLES; //Explosion particle effect

    Rigidbody2D _rbody;

    const float _TTL = 1f; //Molotov's time to live after being thrown
    const float _EXPLOSION_RANGE = 1.5f; //Radius of circle in which damage occurs
    float _start = 0f; //When molotov is spawned

    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _start = Time.time; //Set spawn time
    }

    public override void OnEnemyHit(GameObject enemy) {
        Explode();
    }

    private void FixedUpdate()
    {
        //If molotov has exceeded it's TTL, explode
        if (Time.time - _start >= _TTL)
        {
            Explode();
        }
    }

    private void Explode()
    {
        //Instantiate animation and particle effect
        Instantiate(EXPLOSION_ANIM, _rbody.position, Quaternion.identity);
        Instantiate(EXPLOSION_PARTICLES, _rbody.position, Quaternion.identity);

        List<Collider2D> enemies = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        //Get objects in circular range specified
        int numCollisions = Physics2D.OverlapCircle(_rbody.position, _EXPLOSION_RANGE, filter.NoFilter(), enemies);

        if(numCollisions > 0)
        {
            foreach(Collider2D enemy in enemies)
            {
                //For each collision, if it is an enemy, damage the enemy and add to player's score
                GameObject enemyGameObject = enemy.gameObject;
                if(enemyGameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    EnemyHealthScript.DamageAndScore(enemyGameObject, 40, _playerCreatedBy);
                }
            }
        }
        
        Destroy(gameObject);
    }

}
