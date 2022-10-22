using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

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

}
