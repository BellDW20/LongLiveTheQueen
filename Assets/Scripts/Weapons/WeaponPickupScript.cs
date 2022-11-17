using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class WeaponPickupScript : MonoBehaviour
{
    [SerializeField] private Gun _pickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Gun GetGun()
    {
        return _pickup;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _pickup.Init();
        col.gameObject.GetComponent<PlayerController>().SetPrimaryGun(_pickup);
    }
}
