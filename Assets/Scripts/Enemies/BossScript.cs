using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossScript : MonoBehaviour
{
    public GameObject _minionPrefab;

    Animator _animator;
    Rigidbody2D _rbody;
    Transform _transform;

    public GameObject _enhancement;

    int _currentAttack = 0;
    float _attackCooldown = 1.5f;
    float _attackTimer = 0;
    float _moveSpeed = 5;

    float _rollSpeed = 8;
    float _rollDuration = 8;
    float _rollTimer = 0;

    float _dashStartupDuration = 2;
    float _dashStartupTimer = 0;
    float _dashSpeed = 20;
    int _dashCount = 0;
    bool _isDashing = false;

    float _spawnTimer = 0;
    float _spawnCooldown = 1;
    float _spawnCount = 0;
    float _spawnMax = 5;

    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _currentAttack = 0;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_enhancement == null)
        {
            Destroy(this.gameObject);
        }

        if (_attackCooldown <= 0)
        {
            _attackTimer = Time.time;
            _animator.speed = 0.5f;
            _animator.SetBool("Spawn", true);
        }
        else if (Time.time - _attackTimer > _attackCooldown){
            _animator.speed = 1;
            _animator.SetBool("Spawn", false);

            if (_currentAttack == 0)
            {
                RollAttack();
            }
            else if (_currentAttack == 1)
            {
                DashAttack();
            }
            else if (_currentAttack == 2)
            {
                SpawnAttack();
            }
        }
    }

    void RollAttack()
    {
        _transform.Rotate(0, 0, 100 * Time.deltaTime);

        if (_rollTimer <= 0)
        {
            _rbody.velocity = (new Vector2(-1, -1)) * _rollSpeed;
            _rollTimer = Time.time;

        }
        else if (Time.time - _rollTimer > _rollDuration)
        {
            _rbody.velocity = Vector2.zero;
            _transform.rotation = Quaternion.identity;
            _rollTimer = 0;
            _currentAttack = 1;
            _attackTimer = 0;
        }
    }

    void DashAttack()
    {
        _transform.Rotate(0, 0, 200 * Time.deltaTime);

        if (_dashStartupTimer <= 0)
        {
            _dashStartupTimer = Time.time;
        }
        else if (Time.time - _dashStartupTimer > _dashStartupDuration)
        {
            if (!_isDashing)
            {
                Vector2 temp = MSMScript.NearestPlayerPosition(this.gameObject);
                Vector2 vel = temp - _rbody.position;
                _rbody.velocity = vel.normalized * _dashSpeed;
                _isDashing = true;
            }
        }
    }


    void SpawnAttack()
    {
        _animator.SetBool("Spawn", true);

        if (_spawnTimer <= 0)
        {
            _spawnTimer = Time.time;
        }
        else if (Time.time - _spawnTimer > _spawnCooldown)
        {
            Instantiate(_minionPrefab, _transform.position, Quaternion.identity);
            _spawnCount++;
            _spawnTimer = 0;

            if (_spawnCount >= _spawnMax)
            {
                _animator.SetBool("Spawn", false);
                _currentAttack = 0;
                _attackTimer = 0;
                _spawnCount = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentAttack == 1)
        {
            if (_dashCount < 3)
            {
                _rbody.velocity = Vector2.zero;
                _dashCount++;
                _dashStartupTimer = 0;
                _isDashing = false;
            }
            else
            {
                _dashCount = 0;
                _currentAttack = 2;
                _transform.rotation = Quaternion.identity;
                _attackTimer = 0;
                _isDashing = false;
                _rbody.velocity = _rbody.velocity.normalized * _moveSpeed;
            }
        }

        
    }
}
