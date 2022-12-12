using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorScript : FloatingTextScript {
    protected override void Start() {
        base.Start();
        float angle = Random.Range(0, 2*Mathf.PI);
        _direction = 0.01f * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

}
