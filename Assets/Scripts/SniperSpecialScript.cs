using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SniperSpecialScript : MonoBehaviour
{
    //player ID
    private int _playerCreatedBy;

    Transform _transform;
    GameObject _self;        //Reference to self for duplication when splitting
    Rigidbody2D _rbody;


    int _splitsLeft;         //Number of times the shot has left to split
    bool _isActive = false;  //If shot is active
    float _spreadAngle = 15; //Angle of shot spread in degrees


    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _rbody = GetComponent<Rigidbody2D>();
        _self = this.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActive && _splitsLeft >= 0)
        {

            //Instantiate new bullets
            GameObject tempSpecial1 = Instantiate(_self, _transform.position, Quaternion.identity);
            GameObject tempSpecial2 = Instantiate(_self, _transform.position, Quaternion.identity);


            Vector2 currentVelocity = _rbody.velocity;
            Vector3 temp = new Vector3(currentVelocity.x, currentVelocity.y, 0);

            //Rotate bullet orientation by spreadAngle and applies velocity
            Vector3 vel1 = Quaternion.AngleAxis(_spreadAngle, new Vector3(0, 0, 1)) * temp;
            Vector3 vel2 = Quaternion.AngleAxis(-_spreadAngle, new Vector3(0, 0, 1)) * temp;
            tempSpecial1.GetComponent<Rigidbody2D>().velocity = vel1;
            tempSpecial2.GetComponent<Rigidbody2D>().velocity = vel2;

            //Decrements splitsLeft for the new bullets
            SniperSpecialScript tmpScr = tempSpecial1.GetComponent<SniperSpecialScript>();
            tmpScr.SetSplits(_splitsLeft - 1);
            tmpScr.SetPlayerCreatedBy(_playerCreatedBy);

            tmpScr = tempSpecial2.GetComponent<SniperSpecialScript>();
            tmpScr.SetSplits(_splitsLeft - 1);
            tmpScr.SetPlayerCreatedBy(_playerCreatedBy);

            Cleanup(collision);
        }
        else if(_splitsLeft < 0)
        {
            Cleanup(collision);
        }
    }

    //Applies player scores on collisions with enemies
    private void Cleanup(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            EnemyHealthScript.DamageAndScore(other.gameObject, 20, _playerCreatedBy);
        }
        Destroy(this.gameObject);
    }

    //Prevents bullet from infinitely splitting on the same enemy
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isActive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public void SetSplits(int splits)
    {
        _splitsLeft = splits;
    }

    public void SetIsActive()
    {
        _isActive = true;
    }

    public void SetPlayerCreatedBy(int _playerCreatedBy) {
        this._playerCreatedBy = _playerCreatedBy;
    }
}
