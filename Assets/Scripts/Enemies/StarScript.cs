using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    Transform _transform;
    private float RotateSpeed = 5f;
    private float Radius = 0.2f;

    private Vector2 _centre;
    private float _angle;

    bool _negate = false;

    private void Start()
    {
        _transform = transform;
        _centre = _transform.position;
    }

    private void Update()
    {
        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        if (_negate)
        {
            _transform.position = _centre + (-1 * offset);
        }
        else
        {
            _transform.position = _centre + offset;
        }
    }

    public void NegateOffset()
    {
        _negate = true;
    }
}
