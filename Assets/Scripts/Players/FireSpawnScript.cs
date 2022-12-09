using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawnScript : MonoBehaviour
{
    public GameObject _fire;

    Transform _transform;

    float _lifeTime = 1f;
    float _start;

    float _spawnCooldown = 0.15f;
    float _spawnStart;

    int _playerNum;
    // Start is called before the first frame update
    void Start()
    {
        _start = Time.time;
        _spawnStart = Time.time;
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _start >= _lifeTime)
        {
            Destroy(gameObject);
        }

        if (Time.time - _spawnStart >= _spawnCooldown)
        {
            GameObject temp = Instantiate(_fire, _transform.position, Quaternion.identity);
            temp.GetComponent<FireScript>().SetPlayerNum(_playerNum);
            _spawnStart = Time.time;
        }
    }

    public void SetPNum(int pNum)
    {
        _playerNum = pNum;
    }
}
