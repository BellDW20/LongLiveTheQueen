using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : PlayerProjectileScript
{
    [SerializeField] float _timeToLive;
    [SerializeField] float _tickRate;
    float _lifeTimer, _lastDamageTick;
    int _playerNum;

    private Transform _transform;
    private int ENEMY_LAYER;

    // Start is called before the first frame update
    void Start()
    {
        _lifeTimer = Time.time;
        _transform = transform;
        ENEMY_LAYER = 1 << (LayerMask.NameToLayer("Enemies"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lifeTimer >= _timeToLive)
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerNum(int pNum)
    {
        _playerNum = pNum;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (Time.time - _lastDamageTick < _tickRate) { return; }

            Collider2D[] hit = Physics2D.OverlapCircleAll(_transform.position, 0.45f, ENEMY_LAYER);
            foreach (Collider2D collider in hit) {
                EnemyHealthScript.DamageAndScore(collision.gameObject, DAMAGE, _playerNum);
            }

            _lastDamageTick = Time.time;
        }
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        // :(
    }
}
