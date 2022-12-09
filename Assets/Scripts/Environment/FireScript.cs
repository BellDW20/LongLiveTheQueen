using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : PlayerProjectileScript
{
    [SerializeField] float _timeToLive;
    float _lifeTimer;
    int _playerNum;
    
    // Start is called before the first frame update
    void Start()
    {
        _lifeTimer = Time.time;
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
            EnemyHealthScript.DamageAndScore(collision.gameObject, DAMAGE, _playerNum);
        }
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        // :(
    }
}
