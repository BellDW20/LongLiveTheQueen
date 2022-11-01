using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnhanceScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.Damage(40);
        }
    }
}
