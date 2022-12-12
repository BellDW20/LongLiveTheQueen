using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private const int ST_FOLLOWING = 3;

    //vars
    private Rigidbody2D _rbody;
    private Animations _anim;
    private float _lastChargeTime, _lastDodgeTime;
    private Vector2 _nearestPlayerPos;
    [SerializeField] private BulletDetector _bulletDetector;
    [SerializeField] private SpriteRenderer _swordSpr;
    [SerializeField] private CircleCollider2D _swordCollider;

    private NavMeshAgent _agent;
    private float _lastPathRefresh;

    //State management variables
    private int _state;
    private Action[] stateUpdateFunctions;

    void Start() {
        _rbody = GetComponent<Rigidbody2D>();
        _anim = new Animations(GetComponent<Animator>(), "Walk");
        _lastChargeTime = Time.time+UnityEngine.Random.value*2;
        _agent = GetComponent<NavMeshAgent>();
        stateUpdateFunctions = new Action[] {
            this.CircleUpdate,
            this.ChargeUpdate,
            this.ChargeUpdate,
            this.FollowUpdate
        };
        SetSwordActivation(false);
        _agent.enabled = false;
        _agent.updateRotation = false; //Prevents navmesh agent from rotating in 3 dimensions (!!)
        _agent.updateUpAxis = false; // Prevents navmesh agent from thinking up is in a 3rd dimension (!!)
    }

    void Update() {
        stateUpdateFunctions[_state]();
    }

    private void FollowUpdate() {
        _anim.SetAnimation("Walk");
        if(Time.time - _lastPathRefresh > 0.3) {
            bool inRange = PlayerInRange();
            if (inRange) {
                _agent.enabled = false;
                _state = ST_CIRCLING;
            } else {
                _agent.SetDestination(_nearestPlayerPos);
            }
            _lastPathRefresh = Time.time;
        }

        if (Time.time - _lastDodgeTime > DODGE_DELAY) {
            if (_bulletDetector.IsBulletDetected()) {
                Vector2 bVel = _bulletDetector.ReadLastDetectedBulletDirection();
                float bVelAnglePerp = Mathf.Atan2(bVel.y, bVel.x) + Mathf.PI * 0.5f * (UnityEngine.Random.value > 0.5 ? -1 : 1);
                Vector2 bVelPerp = new Vector2(Mathf.Cos(bVelAnglePerp), Mathf.Sin(bVelAnglePerp));
                _nearestPlayerPos = _rbody.position + CHARGE_SPEED * bVelPerp;
                _lastDodgeTime = Time.time;
                _agent.enabled = false;
                _state = ST_DODGING;
            }
        }
    }

    private void CircleUpdate() {
        _anim.SetAnimation("Walk");
        float dt = Time.time - Mathf.Max(_lastChargeTime, _lastDodgeTime);
        if (dt > CHARGE_DELAY) {
            if(PlayerInRange()) {
                _lastChargeTime = Time.time;
                SetSwordActivation(true);
                SoundManager.PlaySFX(SFX.KNIGHT_SLASH);
                _state = ST_CHARGING;
            } else {
                _agent.enabled = true;
                _state = ST_FOLLOWING;
            }
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

    private bool PlayerInRange() {
        _nearestPlayerPos = MSMScript.NearestPlayerPosition(gameObject);

        return (_nearestPlayerPos - _rbody.position).magnitude < 5f;
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
            SetSwordActivation(false);
            _state = ST_CIRCLING;
        } else {
            _rbody.velocity = ((1 - progress) * CHARGE_SPEED) * ds.normalized;
        }
    }

    private void SetSwordActivation(bool activated) {
        _swordSpr.enabled = activated;
        _swordCollider.enabled = activated;
    }

}
