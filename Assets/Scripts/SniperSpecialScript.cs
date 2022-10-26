using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SniperSpecialScript : MonoBehaviour
{

    private int _playerCreatedBy;

    int _splitsLeft;
    Transform _transform;
    GameObject _self;
    Rigidbody2D _rbody;

    bool _isActive = false;
    float _spreadAngle = 15;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _rbody = GetComponent<Rigidbody2D>();
        _self = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActive && _splitsLeft >= 0)
        {
            GameObject tempSpecial1 = Instantiate(_self, _transform.position, Quaternion.identity);
            GameObject tempSpecial2 = Instantiate(_self, _transform.position, Quaternion.identity);

            Vector2 currentVelocity = _rbody.velocity;
            Vector3 temp = new Vector3(currentVelocity.x, currentVelocity.y, 0);

            Vector3 vel1 = Quaternion.AngleAxis(_spreadAngle, new Vector3(0, 0, 1)) * temp;
            Vector3 vel2 = Quaternion.AngleAxis(-_spreadAngle, new Vector3(0, 0, 1)) * temp;
            tempSpecial1.GetComponent<Rigidbody2D>().velocity = vel1;
            tempSpecial2.GetComponent<Rigidbody2D>().velocity = vel2;


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

    private void Cleanup(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            Enemy.DamageAndScore(other.gameObject, 20, _playerCreatedBy);
        }
        Destroy(this.gameObject);
    }

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
