using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroSpecialScript : PlayerProjectileScript
{
    [SerializeField] GameObject _fire;
    Rigidbody2D _rbody;

    float _lifeTime = 1;
    float _start;

    Vector2 _vel;
    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _start = Time.time;
        _vel = _rbody.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _start >= _lifeTime)
        {
            Detonate();
        }
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        EnemyHealthScript.DamageAndScore(enemy, DAMAGE, _playerCreatedBy);
        SoundManager.PlaySFX(SFX.ENEMY_HIT);
        Detonate();
    }

    private void Detonate()
    {
        Vector2 fireLine = Vector2.Perpendicular(_vel);
    }
}
