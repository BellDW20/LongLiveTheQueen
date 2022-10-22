using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PTestScript : MonoBehaviour
{

    private Rigidbody2D _rbody;

    void Start() {
        _rbody = GetComponent<Rigidbody2D>();
        MSMScript.RegisterPlayer(gameObject);
    }

    private void Update() {
        _rbody.velocity = 5 * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

}
