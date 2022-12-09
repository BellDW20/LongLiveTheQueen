using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] float _timeToLive;
    [SerializeField] float _damage;
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
        if (Time.time - _lifeTimer > _timeToLive)
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerNUm(int pNum)
    {
        _playerNum = pNum;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {        
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealthScript.DamageAndScore(collision.gameObject, _damage, _playerNum);
        }
    }
}
