using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Gun {

    public override void Init() {
        _reloadDelay = 4f;
        _magSize = 100;
        _spreadAngle = 0;
        _shotVelocity = 0;
        base.Init();
    }



}
