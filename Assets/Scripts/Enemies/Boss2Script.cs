using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss2Script : MonoBehaviour
{
    public GameObject _minionPrefab;
    public GameObject _corgiPrefab;
    public GameObject _teaPrefab;
    public GameObject _cupPrefab;
    public GameObject _targetPrefab;

    public GameObject _levelGoal;

    Rigidbody2D _rbody;
    Transform _transform;
    SpriteRenderer _sRender;

    EnemyHealthScript _bossHealth;

    Color32 _startColor = new Color32(194, 146, 229, 160);
    Color32 _phase2Color = new Color32(255, 90, 0, 160);

    Color32 _fullOpacity1 = new Color32(194, 146, 229, 255);
    Color32 _fullOpacity2 = new Color32(255, 90, 0, 255);

    bool _isPhaseTwo = false;

    float _damage = 50;

    float _stunTimer = 0;
    float _stunDuration = 5;
    bool _isStun = false;
    GameObject star1;
    GameObject star2;
    public GameObject _starPrefab;

    int _currentAttack = 0;
    float _attackCooldown = 1.5f;
    float _attackTimer = 0;
    float _moveSpeed = 3;

    GameObject _cup;

    float _corgiDelay = 0.5f;
    int _corgiCount = 10;
    float _corgiTimer;
    float _corgiSpeed = 8;
    float _corgiAngle = 15;
    int _corgiWaves = 4;
    int _corgiCurWave = 0;

    float _spawnTimer = 0;
    float _spawnCooldown = 0.5f;
    float _spawnCount = 0;
    float _spawnMax = 10;

    float _teacupRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        _bossHealth = GetComponent<EnemyHealthScript>();
        _rbody = GetComponent<Rigidbody2D>();
        _sRender = GetComponent<SpriteRenderer>();
        _transform = transform;
        _currentAttack = 0;

        Vector3 starPos = new Vector3(_transform.position.x, _transform.position.y + 0.8f, _transform.position.z);

        _cup = Instantiate(_cupPrefab, _transform.position, Quaternion.identity);
        _cup.transform.SetParent(_transform);
        _cup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPhaseTwo && _bossHealth.GetHealth() < _bossHealth.GetMaxHealth() / 2)
        {
            InitPhaseTwo();
        }


        if (_isStun && Time.time - _stunTimer > _stunDuration)
        {
            _isStun = false;
            _stunTimer = 0;
            Destroy(star1);
            Destroy(star2);
        }

        if (!_isStun)
        {
            Vector2 player = MSMScript.NearestPlayerPosition(gameObject);
            _rbody.velocity = (player - _rbody.position).normalized * _moveSpeed;

            if ((player - _rbody.position).magnitude < 0.25f)
            {
                _rbody.velocity = Vector2.zero;
            }

            if (_attackTimer <= 0)
            {
                _attackTimer = Time.time;
            }
            else if (Time.time - _attackTimer > _attackCooldown)
            {
                if (_currentAttack == 0)
                {
                    CorgiAttack();
                }
                else if (_currentAttack == 1)
                {
                    TeaAttack();
                }
                else if (_currentAttack == 2)
                {
                    AirStrike();
                }
                else if (_currentAttack == 3)
                {
                    SpawnAttack();
                }
            }
        }
    }

    void CorgiAttack()
    {
        if (_corgiCurWave < _corgiWaves)
        {
            if (_corgiTimer <= 0)
            {
                _corgiTimer = Time.time;
            }

            if (Time.time - _corgiTimer >= _corgiDelay)
            {
                GameObject temp1 = Instantiate(_corgiPrefab, _transform.position, Quaternion.identity);
                Rigidbody2D tempRbody = temp1.GetComponent<Rigidbody2D>();
                tempRbody.velocity = (MSMScript.NearestPlayerPosition(gameObject) - tempRbody.position).normalized * _corgiSpeed;


                for (int i = 0; i < _corgiCount; i++)
                {
                    GameObject temp = Instantiate(_corgiPrefab, _transform.position, Quaternion.identity);

                    float shotAngle = Mathf.Atan2(
                        tempRbody.velocity.y,
                        tempRbody.velocity.x
                        ) + GetSpreadAngle();

                    Vector2 shotDirection = new Vector2(Mathf.Cos(shotAngle), Mathf.Sin(shotAngle));

                    temp.transform.up = shotDirection;

                    temp.GetComponent<Rigidbody2D>().velocity = _corgiSpeed * shotDirection;
                }

                _corgiCurWave++;
                _corgiTimer = float.NegativeInfinity;
            }
        }
        else
        {
            if (_isPhaseTwo)
            {
                _currentAttack = RandomizeAttack();
            }
            else
            {
                _currentAttack++;
            }
            _attackTimer = 0;
            _corgiCurWave = 0;
        }
    }
    float GetSpreadAngle()
    {
        return Mathf.PI * Random.Range(-_corgiAngle, _corgiAngle) / 180.0f;
    }

    void TeaAttack()
    {
        if (!_cup.activeInHierarchy) {
            _teacupRotation = 0;
            _cup.SetActive(true);
        }

        _teacupRotation += Time.deltaTime * 90;
        _cup.transform.rotation = Quaternion.Euler(new Vector3(0,0,_teacupRotation));

        if (_teacupRotation >= 180)
        {
            Instantiate(_teaPrefab, _transform.position, Quaternion.identity);

            if (_isPhaseTwo)
            {
                _currentAttack = RandomizeAttack();
            }
            else
            {
                _currentAttack++;
            }
            
            _attackTimer = 0;
            _cup.SetActive(false);
        }
    }

    void AirStrike()
    {
        LinkedList<GameObject> players = MSMScript.GetPlayers();

        LinkedListNode<GameObject> temp = players.First;

        for (int i = 0; i < players.Count; i++)
        {
            Instantiate(_targetPrefab, temp.Value.transform.position, Quaternion.identity);
            temp = temp.Next;
        }

        if (_isPhaseTwo)
        {
            _currentAttack = RandomizeAttack();
        }
        else
        {
            _currentAttack++;
        }
        _attackTimer = 0;
    }


    void SpawnAttack()
    {
        if (_isPhaseTwo)
        {
            _sRender.color = _fullOpacity2;
        }
        else
        {
            _sRender.color = _fullOpacity1;
        }

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
                if (_isPhaseTwo)
                {
                    _currentAttack = RandomizeAttack();
                }
                else
                {
                    _currentAttack = 0;
                }

                _attackTimer = 0;
                _spawnCount = 0;

                if (_isPhaseTwo)
                {
                    _sRender.color = _phase2Color;
                }
                else
                {
                    _sRender.color = _startColor;
                }
            }
        }

    }

    public void Stun()
    {
        _isStun = true;
        if (_isPhaseTwo)
        {
            _currentAttack = RandomizeAttack();
        }
        else
        {
            if (_currentAttack == 3)
            {
                _currentAttack = 0;
            }
            else
            {
                _currentAttack++;
            }
        }

        _attackTimer = 0;
        _spawnCount = 0;
        _rbody.velocity = Vector2.zero;
        _stunTimer = Time.time;
        Vector3 starPos = new Vector3(_transform.position.x, _transform.position.y + 0.8f, _transform.position.z);
        star1 = Instantiate(_starPrefab, starPos, Quaternion.Euler(new Vector3(0, 0, 45)));
        star2 = Instantiate(_starPrefab, starPos, Quaternion.Euler(new Vector3(0, 0, 45)));
        star2.GetComponent<StarScript>().NegateOffset();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Damage(_damage);
        }
    }

    private void OnDestroy()
    {
        if (_levelGoal != null)
        {
            _levelGoal.SetActive(true);
        }
    }

    void InitPhaseTwo()
    {
        if (!_isPhaseTwo)
        {
            _currentAttack = -1;
            StartCoroutine(ColorChange(_phase2Color));
            _isPhaseTwo = true;
            _spawnMax = 15;
            _spawnCooldown = 0.3f;
            _corgiWaves = 6;
            _corgiDelay = 0.4f;
            _corgiAngle = 20;
            _attackCooldown = 0.75f;
            _moveSpeed = 4;
        }
    }

    IEnumerator ColorChange(Color targetColor)
    {
        float tick = 0f;
        while (_sRender.color != targetColor)
        {
            tick += Time.deltaTime * 0.25f;
            _sRender.color = Color.Lerp(_startColor, targetColor, tick);
            yield return null;
        }

        _currentAttack = RandomizeAttack();
    }

    int RandomizeAttack()
    {
        return Random.Range(0, 4);
    }
}
