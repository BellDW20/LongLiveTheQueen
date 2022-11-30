using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletScript : PlayerProjectileScript
{
    float _spreadAngle = 20;
    int _shotCount = 6;

    bool _clone = true;

    Rigidbody2D _rbody;

    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_clone && _rbody.velocity != Vector2.zero)
        {
            for (int i = 0; i < _shotCount; i++)
            {
                GameObject tempBullet = Object.Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
                tempBullet.GetComponent<ShotgunBulletScript>().SetClone();

                float shotAngle = Mathf.Atan2(
                _rbody.velocity.y,
                _rbody.velocity.x
                ) + GetSpreadAngle();

                Vector2 shotDirection = new Vector2(Mathf.Cos(shotAngle), Mathf.Sin(shotAngle));

                tempBullet.transform.up = shotDirection;

                //Set velocity and the player who created the shot
                tempBullet.GetComponent<Rigidbody2D>().velocity = 8 * shotDirection;
            }

            SetClone();
        }
    }

    float GetSpreadAngle()
    {
        return Mathf.PI * Random.Range(-_spreadAngle, _spreadAngle) / 180.0f;
    }

    public void SetClone()
    {
        _clone = false;
    }
}
