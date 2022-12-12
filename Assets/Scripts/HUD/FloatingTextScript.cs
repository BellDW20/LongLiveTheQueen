using UnityEngine;
using UnityEngine.UI;

public class FloatingTextScript : MonoBehaviour {

    [SerializeField] private float TIME_TO_LIVE;
    [SerializeField] private Text _text;
    [SerializeField] protected Vector2 _direction;
    private Vector3 _offset;
    private float _timeCreated;
    private Transform _trackWith = null;

    protected Transform _transform;
    protected virtual void Start() {
        _timeCreated = Time.time;
        _transform = transform;
    }

    protected virtual void Update() {
        float d = Mathf.Clamp01((1-((Time.time - _timeCreated) / TIME_TO_LIVE)));
        Vector2 ds = d * _direction;
        _transform.position = (_trackWith != null ? _trackWith.position : _transform.position) + new Vector3(ds.x, ds.y, 0) + _offset;

        if (Time.time - _timeCreated > TIME_TO_LIVE) {
            Destroy(gameObject);
        }
    }

    public void SetText(string text) {
        _text.text = text;
    }

    public void SetTrackingWith(Transform transform) {
        _trackWith = transform;
    }

    public void SetOffset(Vector2 offset) {
        _offset = offset;
    }

}