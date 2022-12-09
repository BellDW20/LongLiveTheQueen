using UnityEngine;

public class PyroFlameParticle : MonoBehaviour {

    private const float TIME_TO_LIVE = 0.2f;

    [SerializeField] private SpriteRenderer _spr;
    private ObjectPool _pool;
    private float _timeCreated;

    public void Update() {
        float dt = Time.time - _timeCreated;

        if (dt > TIME_TO_LIVE) {
            _pool.Return(gameObject);
        } else {
            _spr.color = new Color(1, 1, 1, 1 - dt / TIME_TO_LIVE);
        }
    }

    public void Reset(ObjectPool pool) {
        _pool = pool;
        _timeCreated = Time.time;
        _spr.color = Color.white;
    }

}