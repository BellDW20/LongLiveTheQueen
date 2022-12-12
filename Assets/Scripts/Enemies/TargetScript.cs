using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public GameObject EXPLOSION_ANIM; //Explosion animation
    public GameObject EXPLOSION_PARTICLES; //Explosion particle effect

    Rigidbody2D _rbody;

    const float _EXPLOSION_RANGE = 1f; //Radius of circle in which damage occurs

    int _damage = 80;

    SpriteRenderer _sRender;

    Color _startColor;

    float _colorSpeed = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        _sRender = GetComponent<SpriteRenderer>();
        _rbody = GetComponent<Rigidbody2D>();
        _startColor = _sRender.color;

        StartCoroutine(ColorChange(Color.black));
    }

    // Update is called once per frame
    void Update()
    {
        if (_sRender.color == Color.black)
        {
            Explode();
        }
    }

    IEnumerator ColorChange(Color targetColor)
    {
        float tick = 0f;
        while (_sRender.color != targetColor)
        {
            tick += Time.deltaTime * _colorSpeed;
            _sRender.color = Color.Lerp(_startColor, targetColor, tick);
            yield return null;
        }
    }

    void Explode()
    {
        //Instantiate animation and particle effect
        Instantiate(EXPLOSION_ANIM, _rbody.position, Quaternion.identity);
        Instantiate(EXPLOSION_PARTICLES, _rbody.position, Quaternion.identity);

        //Get objects in circular range specified
        int enLayer = LayerMask.NameToLayer("Enemies");
        int plLayer = LayerMask.NameToLayer("Players");
        int layerMask = (1 << enLayer) | (1 << plLayer);
        Collider2D[] hits = Physics2D.OverlapCircleAll(_rbody.position, _EXPLOSION_RANGE, layerMask);

        foreach (Collider2D enemy in hits)
        {
            //For each collision, if it is an enemy, damage the enemy and add to player's score
            GameObject enemyGameObject = enemy.gameObject;
            if (enemyGameObject.layer == enLayer)
            {
                if (MSMScript.NearestPlayer(gameObject) != null)
                {
                    EnemyHealthScript.DamageAndScore(enemyGameObject, _damage, MSMScript.NearestPlayer(gameObject).GetComponent<PlayerController>().GetPlayerNumber());
                }

                if (enemyGameObject.CompareTag("Boss"))
                {
                    if (enemyGameObject.GetComponent<Boss2Script>() != null)
                    {
                        enemyGameObject.GetComponent<Boss2Script>().Stun();
                    }
                }
            }
            else if (enemyGameObject.layer == plLayer)
            {
                if (enemyGameObject.GetComponent<PlayerController>() != null)
                {
                    enemyGameObject.GetComponent<PlayerController>().Damage(_damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
