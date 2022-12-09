using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroSpecialScript : PlayerProjectileScript
{
    [SerializeField] GameObject _tempBullet;
    [SerializeField] GameObject _fire;
    Rigidbody2D _rbody;

    Transform _transform;

    float _lifeTime = 0.75f;
    float _start;

    Vector2 _vel;
    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _transform = transform;
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
        Instantiate(_fire, _transform.position, Quaternion.identity);
        GameObject temp1 = Instantiate(_tempBullet, _transform.position, Quaternion.Euler(0, 0, 90));
        GameObject temp2 = Instantiate(_tempBullet, _transform.position, Quaternion.Euler(0, 0, 270));
        temp1.GetComponent<Rigidbody2D>().velocity = fireLine;
        temp2.GetComponent<Rigidbody2D>().velocity = fireLine * -1;
        temp1.GetComponent<FireSpawnScript>().SetPNum(_playerCreatedBy);
        temp2.GetComponent<FireSpawnScript>().SetPNum(_playerCreatedBy);
        Destroy(gameObject);
    }
}
