using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroBulletScript : PlayerProjectileScript {

    [SerializeField] private GameObject FLAMETHROWER_PARTICLES;

    // Start is called before the first frame update
    void Start() {
        Invoke("Die", 0.05f);
        GameObject particles = Instantiate(FLAMETHROWER_PARTICLES, transform.position, transform.rotation);
        particles.transform.Rotate(new Vector3(-90, 0, 0));
        particles.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        EnemyHealthScript.DamageAndScore(enemy, DAMAGE, _playerCreatedBy);
        SoundManager.PlaySFX(SFX.ENEMY_HIT);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
