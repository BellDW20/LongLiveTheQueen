using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CommandoSpecialScript : MonoBehaviour
{
    public GameObject EXPLOSION_ANIM;
    public GameObject EXPLOSION_PARTICLES;

    Rigidbody2D _rbody;

    const float _TTL = 1f; //Molotov's time to live after being thrown
    const float _EXPLOSION_RANGE = 1.5f;
    float _start = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _start = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time - _start >= _TTL)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(EXPLOSION_ANIM, _rbody.position, Quaternion.identity);
        Instantiate(EXPLOSION_PARTICLES, _rbody.position, Quaternion.identity);

        List<Collider2D> enemies = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        int numCollisions = Physics2D.OverlapCircle(_rbody.position, _EXPLOSION_RANGE, filter.NoFilter(), enemies);

        if(numCollisions > 0)
        {
            foreach(Collider2D enemy in enemies)
            {
                GameObject enemyGameObject = enemy.gameObject;
                if(enemyGameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    enemyGameObject.GetComponent<EnemyHealthScript>().Damage(40);
                }
            }
        }
        
        Destroy(gameObject);
    }
}
