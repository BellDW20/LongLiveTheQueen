using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberScript : MonoBehaviour {

    public float SHOT_COOLDOWN;
    public float SHOT_SPEED;
    public Transform MUZZLE_POS;
    public GameObject BOMB_PREFAB;

    private float lastTimeShot;
    private Transform _transform;

    void Start() {
        lastTimeShot = Time.time;
        _transform = GetComponent<Transform>();
    }

    void Update() {
        if(Time.time - lastTimeShot > SHOT_COOLDOWN) {
            lastTimeShot = Time.time;
            GameObject nearestPlayer = MSMScript.NearestPlayer(gameObject);

            if (nearestPlayer != null) {
                ShootBombAt(nearestPlayer);
            }
        }
    }

    private void ShootBombAt(GameObject player) {
        GameObject bombObj = Instantiate(BOMB_PREFAB, MUZZLE_POS.position, Quaternion.identity);
        BombScript bombScr = bombObj.GetComponent<BombScript>();
        bombScr.target = CalculateTargetPos(player);
        bombScr.speed = SHOT_SPEED;
    }

    private Vector2 CalculateTargetPos(GameObject player) {
        Rigidbody2D _rb = player.GetComponent<Rigidbody2D>();

        float estDistToTravel = (_rb.position - new Vector2(MUZZLE_POS.position.x, MUZZLE_POS.position.y)).magnitude;
        float estTimeToTravel = estDistToTravel / SHOT_SPEED;

        return _rb.position + 0.5f * estTimeToTravel * _rb.velocity;
    }

}
