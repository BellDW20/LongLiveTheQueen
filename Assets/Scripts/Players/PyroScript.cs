using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroScript : PlayerController {

    private const float TIME_TO_SHRINK = 1;

    private float _shootTimer = 0;
    private float _startSize = 0.25f;
    private float _endSize = 0.1f;

    public override void Start() {
        base.Start();
    }

    public override void Update() {
        base.Update();
    }

    public override void HandleShooting() {
        base.HandleShooting();
        Gun _primary = GetPrimaryGun();

        if (_primary.IsShooting()) {
            _shootTimer += Time.deltaTime;
            float scale = Mathf.Lerp(_startSize, _endSize, Mathf.Clamp(_shootTimer / TIME_TO_SHRINK, 0, 1));
            _primary.GetProjectile().transform.localScale = new Vector3(scale, scale, 1);
        } else {
            _shootTimer = 0;
        }
    }
}
