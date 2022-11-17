using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroBulletScript : PlayerProjectileScript
{
    public GameObject _childSquare;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", 0.05f);
        //Transform childTransform = _childSquare.transform;
        //PolygonCollider2D pCollide = GetComponent<PolygonCollider2D>();
        //childTransform.localScale = new Vector3(pCollide.points[2].x - pCollide.points[0].x, pCollide.points[0].y, transform.localScale.z);
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
