using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroScript : PlayerController {

    private const float TIME_TO_SHRINK = 1;
    private readonly Vector2 START_SIZE = new Vector2(-1, 20);
    private readonly Vector2 END_SIZE = new Vector2(-5, 7);

    private PolygonCollider2D temp;
    private float _shootTimer = 0;

    [SerializeField] private GameObject _flameParticle;
    private ObjectPool _flamethrowerParticles;

    public override void Start() {
        base.Start();
        temp = _primaryGun.GetProjectile().GetComponent<PolygonCollider2D>();

        _flamethrowerParticles = new ObjectPool(_flameParticle, true, 25);
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

            GameObject flame = _flamethrowerParticles.Loan();
            flame.GetComponent<PyroFlameParticle>().Reset(_flamethrowerParticles);
            flame.transform.position = transform.TransformPoint(RandomFlamePos() * 0.25f);
        } else {
            _shootTimer = 0;
        }
    }

    //Generates a uniformly distributed random point
    //in the flamethrower's current spread triangle
    private Vector2 RandomFlamePos() {
        float sr1 = Mathf.Sqrt(Random.value);
        float r2 = Random.value;
        return (1-sr1)*temp.points[0] + sr1*(1-r2)*temp.points[1] + sr1*r2*temp.points[2];
    }

}
