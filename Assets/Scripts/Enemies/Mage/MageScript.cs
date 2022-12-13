using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageScript : MonoBehaviour {

    private const float GROUP_DETECTION_RADIUS = 4;
    private const float HEAL_LIKELIHOOD = 0.5f;

    private Transform _transform;
    private Vector2 _target;
    [SerializeField] private GameObject _healParticles;
    [SerializeField] private GameObject _healAmtParticles;
    [SerializeField] private LayerMask _enemyMask;
    private List<GameObject> _enemiesInRange;

    private Animations _animations;


    void Start() {
        _transform = transform;
        _enemiesInRange = new List<GameObject>();
        _animations = new Animations(GetComponent<Animator>(), "TeleportSpinup");
    }

    public void Teleport() {
        if (IsGroupNearby()) {
            _animations.SetAnimation("HealSpell");
        }
        else {
            _animations.SetAnimation("PlayerAttack");
        }
        _transform.position = new Vector3(_target.x, _target.y, 0);
    }

    private void GroupHeal() {
        foreach (GameObject enemy in _enemiesInRange) {
            if(Random.value < HEAL_LIKELIHOOD && enemy != null) {
                enemy.GetComponent<EnemyHealthScript>().Heal(20);
                GameObject _healAmt = Instantiate(_healAmtParticles, enemy.transform.position, Quaternion.identity);
                _healAmt.GetComponent<DamageIndicatorScript>().SetText("+20");
                Instantiate(_healParticles, enemy.transform.position, Quaternion.identity);
            }
        }
    }

    private void TargetPlayer() {
        _target = MSMScript.NearestPlayerPosition(gameObject);
    }

    private void AttackPlayer() {
        Idle();
    }

    private void Idle() {
        _animations.SetAnimation("TeleportSpinup");
    }

    private bool IsGroupNearby() {
        if(FindEnemiesInRange() == 0) {
            Vector3 pPos = MSMScript.NearestPlayerPosition(gameObject);
            if (LevelManagerScript.GetMode() == GameMode.HORDE_MODE) {
                _target = pPos;
            } else if ((_transform.position - pPos).magnitude < 9f) {
                _target = pPos;
            }
            return false;
        }

        _target = Vector2.zero;
        foreach(GameObject _enemy in _enemiesInRange) {
            _target += (Vector2)_enemy.transform.position;
        }

        _target *= (1.0f / _enemiesInRange.Count);
        return true;
    }

    private int FindEnemiesInRange() {
        Collider2D[] hit = Physics2D.OverlapCircleAll(
            new Vector2(_transform.position.x, _transform.position.y),
            GROUP_DETECTION_RADIUS,
            _enemyMask
        );

        _enemiesInRange.Clear();
        for (int i = 0; i < hit.Length; i++) {
            if (hit[i].CompareTag("Mage")) { continue; }
            _enemiesInRange.Add(hit[i].gameObject);
        }

        return _enemiesInRange.Count;
    }

}
