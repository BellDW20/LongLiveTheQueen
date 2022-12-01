using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class WeaponPickupScript : MonoBehaviour
{
    [SerializeField] private Gun _pickup;

    public Gun GetGun()
    {
        return _pickup;
    }

    public void SetGun(Gun g)
    {
        _pickup = g;
    }
}
