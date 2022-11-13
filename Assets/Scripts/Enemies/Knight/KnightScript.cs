using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnightScript : MonoBehaviour {

    //Constants
    private const float CHARGE_DELAY = 1f;
    private const float CHARGE_DURATION = 0.75f;
    private const float CHARGE_SPEED = 13f;

    private const float CIRCLE_PERIOD = 1f;
    private const float CIRCLE_RADIUS = 3f;

    private const float DODGE_DELAY = 2f;

    //State IDs
    private const int ST_CIRCLING = 0;
    private const int ST_CHARGING = 1;
    private const int ST_DODGING = 2;

    //vars
    private Rigidbody2D _rbody;
    private Animations _anim;
    private float _lastChargeTime, _lastDodgeTime;
    private Vector2 _nearestPlayerPos;
    [SerializeField] private BulletDetector _bulletDetector;
    [SerializeField] private GameObject _sword;

    //State management variables
    private int _state;
    private Action[] stateUpdateFunctions;

    void Start() {
        _rbody = GetComponent<Rigidbody2D>();
        _anim = new Animations(GetComponent<Animator>(), "Walk");
        _lastChargeTime = Time.time;

        stateUpdateFunctions = new Action[] {
            this.CircleUpdate,
            this.ChargeUpdate,
            this.ChargeUpdate
        };
    }

    void Update() {
        stateUpdateFunctions[_state]();
    }

    private void CircleUpdate() {
        _anim.SetAnimation("Walk");
        float dt = Time.time - Mathf.Max(_lastChargeTime, _lastDodgeTime);
        if (dt > CHARGE_DELAY) {
            _nearestPlayerPos = MSMScript.NearestPlayerPosition(gameObject);
            _lastChargeTime = Time.time;
            _sword.SetActive(true);
            SoundManager.PlaySFX(SFX.KNIGHT_SLASH);
            _state = ST_CHARGING;
        } else {
            dt *= 2*Mathf.PI / CIRCLE_PERIOD;
            _rbody.velocity = CIRCLE_RADIUS * new Vector2(-Mathf.Sin(dt), Mathf.Cos(dt));

            if (Time.time - _lastDodgeTime > DODGE_DELAY) {
                if (_bulletDetector.IsBulletDetected()) {
                    Vector2 bVel = _bulletDetector.ReadLastDetectedBulletDirection();
                    float bVelAnglePerp = Mathf.Atan2(bVel.y, bVel.x) + Mathf.PI * 0.5f * (UnityEngine.Random.value > 0.5 ? -1 : 1);
                    Vector2 bVelPerp = new Vector2(Mathf.Cos(bVelAnglePerp), Mathf.Sin(bVelAnglePerp));
                    _nearestPlayerPos = _rbody.position + CHARGE_SPEED * bVelPerp;
                    _lastDodgeTime = Time.time;
                    _state = ST_DODGING;
                }
            }
        }
    }

    private void ChargeUpdate() {
        _anim.SetAnimation("Charge");
        Vector2 ds = (_nearestPlayerPos - _rbody.position);
        _rbody.SetRotation(180 * Mathf.Atan2(ds.y,ds.x) / Mathf.PI - 90);
        float progress = (Time.time - (_state==ST_CHARGING ? _lastChargeTime : _lastDodgeTime)) / CHARGE_DURATION;
        if(progress >= 1 || ds.magnitude < 0.1f) {
            _rbody.velocity = Vector2.zero;
            if (_state == ST_CHARGING) {
                _lastChargeTime = Time.time;
            } else {
                _bulletDetector.ResetDetector();
                _lastDodgeTime = Time.time;
            }
            _sword.SetActive(false);
            _state = ST_CIRCLING;
        } else {
            _rbody.velocity = ((1 - progress) * CHARGE_SPEED) * ds.normalized;
        }
    }

}
