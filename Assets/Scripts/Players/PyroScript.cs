using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroScript : PlayerController
{
    Gun _primary;

    PolygonCollider2D temp;

    float _endy = 7;
    float _endx = 5;


    private const float TIME_TO_SHRINK = 1;

    private float _shootTimer = 0;
    private Vector2 _startSize = new Vector2(-1, 20);
    private Vector2 _endSize = new Vector2(-5, 7);
    public override void Start()
    {
        base.Start();
        _primary = GetPrimaryGun();
        temp = _primary.GetProjectile().GetComponent<PolygonCollider2D>();
    }

    public override void Update() {
        base.Update();
    }

    public override void HandleShooting() {
        base.HandleShooting();
        Gun _primary = GetPrimaryGun();

        if (_primary.IsShooting())
        {
            _shootTimer += Time.deltaTime;
            Vector2 scale = Vector2.Lerp(_startSize, _endSize, Mathf.Clamp(_shootTimer / TIME_TO_SHRINK, 0, 1));
            //_primary.GetProjectile().transform.localScale = new Vector3(scale, scale, 1);
            temp.points = new[] { scale,
                new Vector2(0, 0.25f), new Vector2(-scale.x, scale.y)
                };

        }
        else
        {
            _shootTimer = 0;
        }

        //if (_primary.IsShooting())
        //{
        //    Vector2[] colliderPoints = _tempCone.GetComponent<PolygonCollider2D>().points;

        //    Vector2 tempPoint0 = colliderPoints[0];
        //    Vector2 tempPoint2 = colliderPoints[2];

        //    if (tempPoint0.x >= -_endx)
        //    {
        //        _tempCone.GetComponent<PolygonCollider2D>().points = new[] { new Vector2(tempPoint0.x - 0.08f, tempPoint0.y - 0.26f),
        //        colliderPoints[1], new Vector2(tempPoint2.x + 0.08f, tempPoint2.y - 0.26f)
        //        };

        //        _primary.SetProjectile(_tempCone);
        //    }
        //}
    }
}
