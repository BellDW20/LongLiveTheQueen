using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossScript : MonoBehaviour
{
    public GameObject _minionPrefab;

    Rigidbody2D _rbody;
    Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
