using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorScript : MonoBehaviour {

    private const float TIME_TO_LIVE = 0.5f;

    public Text _damageText;
    private Transform _transform;
    private float _timeCreated;
    private Vector2 _vel;

    public void Start() {
        _timeCreated = Time.time;
        _transform = transform;

        float angle = Random.Range(0, 2*Mathf.PI);
        _vel = 3 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public void Update() {
        float d = Time.deltaTime * ((Time.time - _timeCreated) / TIME_TO_LIVE);
        Vector2 ds = d * _vel;
        _transform.position = new Vector3(_transform.position.x + ds.x, _transform.position.y + ds.y, _transform.position.z);

        if(Time.time - _timeCreated > TIME_TO_LIVE) {
            Destroy(gameObject);
        }
    }

}
