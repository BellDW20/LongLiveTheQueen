using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgiScript : MonoBehaviour
{
    float _damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            collision.GetComponent<PlayerController>().Damage(_damage);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
