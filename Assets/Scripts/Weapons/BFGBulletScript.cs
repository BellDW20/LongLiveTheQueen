using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFGBulletScript : PlayerProjectileScript
{
    Transform _transform;

    public Material _lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEnemyHit(GameObject enemy)
    {
        EnemyHealthScript.DamageAndScore(enemy, DAMAGE, _playerCreatedBy);
        SoundManager.PlaySFX(SFX.ENEMY_HIT);
        GameObject temp = new GameObject();
        LineRenderer _lRender = temp.AddComponent<LineRenderer>();
        _lRender.sortingLayerName = "Entities";
        _lRender.material = _lineMaterial;
        _lRender.startColor = Color.green;
        _lRender.endColor = Color.green;
        _lRender.SetPosition(0, _transform.position);
        _lRender.SetPosition(1, enemy.transform.position);
        _lRender.startWidth = 0.1f;
        _lRender.endWidth = 0.1f;
        StartCoroutine(DisableLine(temp));
    }

    IEnumerator DisableLine(GameObject o)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(o);
    }
}
