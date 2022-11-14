using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroScript : PlayerController
{
    GameObject _startingCone;
    GameObject _tempCone;
    Gun _primary;

    float _shootTimer = float.NegativeInfinity;
    float _shootDelay = 0.05f;

    float _endy = 7;
    float _endx = 5;
    public override void Start()
    {
        base.Start();
        _primary = GetPrimaryGun();
        _startingCone = GetPrimaryGun().GetProjectile();
        _tempCone = _startingCone;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void HandleShooting()
    {
        base.HandleShooting();

        if (!_primary.IsShooting() && _primary.GetProjectile() != _startingCone)
        {
            _primary.SetProjectile(_startingCone);
        }
        else
        {
            if (_shootTimer <= 0)
            {
                _shootTimer = Time.time;
            }
            else if (_primary.IsShooting())
            {
                Vector2 tempPoint0 = _tempCone.GetComponent<PolygonCollider2D>().points[0];
                Vector2 tempPoint2 = _tempCone.GetComponent<PolygonCollider2D>().points[2];


                SpriteRenderer sRender = _tempCone.GetComponent<SpriteRenderer>();

                if (tempPoint0.x != _endx)
                {
                    _tempCone.GetComponent<PolygonCollider2D>().points[0] = new Vector2(tempPoint0.x + 0.08f, tempPoint0.y - 0.26f);
                    _tempCone.GetComponent<PolygonCollider2D>().points[2] = new Vector2(tempPoint2.x + 1.1f, tempPoint2.y - 0.26f);
                    sRender.transform.position = new Vector3(tempPoint0.x, tempPoint0.y, sRender.transform.position.z);
                    _primary.SetProjectile(_tempCone);
                }
            }
        }
    }
}
