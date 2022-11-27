using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroScript : PlayerController {

    private const float TIME_TO_SHRINK = 1;
    private readonly Vector2 START_SIZE = new Vector2(-1, 20);
    private readonly Vector2 END_SIZE = new Vector2(-5, 7);

    private Gun _primary;
    private PolygonCollider2D temp;
    private float _shootTimer = 0;

    public override void Start() {
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

        if (_primary.IsShooting()) {
            _shootTimer += Time.deltaTime;
            Vector2 scale = Vector2.Lerp(START_SIZE, END_SIZE, Mathf.Clamp(_shootTimer / TIME_TO_SHRINK, 0, 1));
            temp.points = new[] { scale,
                new Vector2(0, 0.25f), new Vector2(-scale.x, scale.y)
            };
        } else {
            _shootTimer = 0;
        }

    }
}
