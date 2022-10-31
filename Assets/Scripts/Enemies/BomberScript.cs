using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberScript : MonoBehaviour {

    public float SHOT_COOLDOWN; //Time interval between shots
    public float SHOT_SPEED; //The velocity of bombs shot
    public float BOMBER_RANGE; //The furthest a target can be and still be shot at
    public Transform MUZZLE_POS; //A sub-position which refers to the position of the muzzle of the cannon
    public GameObject BOMB_PREFAB; //Prefab of the bombs which are shot

    private float _lastTimeShot; //The last time a bomb was shot
    private bool _offscreen; //Whether or not this bomber is offscreen
    private Transform _transform;

    void Start() {
        //Set the last time shot to now plus some random amount
        //so that bombers don't all shoot at the exact same time
        _lastTimeShot = Time.time+Random.Range(0,0.8f);
        _transform = GetComponent<Transform>();
        _offscreen = true; //Initially assume we are offscreen
    }

    void Update() {
        //If enough time has passed since the last shot
        if(Time.time - _lastTimeShot > SHOT_COOLDOWN) {
            _lastTimeShot = Time.time; //reset our shot timer
            GameObject nearestPlayer = MSMScript.NearestPlayer(gameObject); //find the nearest player

            //If we're actually on screen and there is a nearest player within range, then shoot
            if (!_offscreen && nearestPlayer != null && (_transform.position - nearestPlayer.transform.position).magnitude < BOMBER_RANGE) {
                ShootBombAt(nearestPlayer);
            }
        }
    }

    private void ShootBombAt(GameObject player) {
        //Create the bomb at the muzzle
        GameObject bombObj = Instantiate(BOMB_PREFAB, MUZZLE_POS.position, Quaternion.identity);
        BombScript bombScr = bombObj.GetComponent<BombScript>();

        //Give the bomb a target and a speed
        bombScr.target = CalculateTargetPos(player);
        bombScr.speed = SHOT_SPEED;
    }

    private Vector2 CalculateTargetPos(GameObject player) {
        Rigidbody2D _rb = player.GetComponent<Rigidbody2D>();

        //Estimate the distance this bomb would travel
        //if shot directly at the player's current position
        float estDistToTravel = (_rb.position - new Vector2(MUZZLE_POS.position.x, MUZZLE_POS.position.y)).magnitude;
        //Calculate the time it would take to travel that distance
        float estTimeToTravel = estDistToTravel / SHOT_SPEED;

        //Then aim at where we would expect the player to be about that time in the future
        //This method rudimently "predicts" player movement for more accurate shots
        return _rb.position + 0.5f * estTimeToTravel * _rb.velocity;
    }

    private void OnBecameVisible() {
        //If we come on screen, make note of that
        _offscreen = false;
    }

    private void OnBecameInvisible() {
        //If we go offscreen, make note of that
        _offscreen = true;
    }

}
