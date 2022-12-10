using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    public GameObject EXPLOSION_ANIM; //Explosion animation
    public GameObject EXPLOSION_PARTICLES; //Explosion particle effect

    Rigidbody2D _rbody;

    const float _EXPLOSION_RANGE = 3f; //Radius of circle in which damage occurs

    int _damage = 80;

    int _playerNum;
    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectiles"))
        {
            _playerNum = collision.GetComponent<PlayerProjectileScript>().GetPlayerCreatedBy();
            Destroy(collision.gameObject);
            Explode();
        }
    }

    void Explode()
    {
        //Instantiate animation and particle effect
        Instantiate(EXPLOSION_ANIM, _rbody.position, Quaternion.identity);
        Instantiate(EXPLOSION_PARTICLES, _rbody.position, Quaternion.identity);

        //Get objects in circular range specified
        int enLayer = LayerMask.NameToLayer("Enemies");
        int plLayer = LayerMask.NameToLayer("Players");
        int layerMask = (1 << enLayer) | (1 << plLayer);
        Collider2D[] hits = Physics2D.OverlapCircleAll(_rbody.position, _EXPLOSION_RANGE, layerMask);

        foreach (Collider2D enemy in hits)
        {
            //For each collision, if it is an enemy, damage the enemy and add to player's score
            GameObject enemyGameObject = enemy.gameObject;
            if (enemyGameObject.layer == enLayer)
            {
                EnemyHealthScript.DamageAndScore(enemyGameObject, _damage, _playerNum);
            } 
            else if (enemyGameObject.layer == plLayer)
            {
                enemyGameObject.GetComponent<PlayerController>().Damage(_damage);
            }
        }

        Destroy(gameObject);
    }
}
