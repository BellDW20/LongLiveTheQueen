using UnityEngine;

public class StagedFightArea : MonoBehaviour {

    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private CameraScript _camera;
    [SerializeField] private Transform _lockPos;
    private BoxCollider2D _collider;
    private int PLAYER_MASK;

    private bool _triggered;

    public void Start() {
        _triggered = false;

        _collider = GetComponent<BoxCollider2D>();
        PLAYER_MASK = 1 << (LayerMask.NameToLayer("Players"));

        foreach (GameObject en in _enemies) {
            en.SetActive(false);
        }
    }

    public void Update() {
        if (!_triggered) { return; }
        bool cleared = true;
        for(int i=0; i<_enemies.Length; i++) {
            cleared &= (_enemies[i] == null);
        }
        if(cleared) {
            _camera.UnlockCamera();
            Destroy(gameObject);
        }
    }

    private bool AllPlayersInside() {
        Collider2D[] playersInside = Physics2D.OverlapBoxAll(_collider.bounds.center, _collider.bounds.size, 0, PLAYER_MASK);
        return playersInside.Length == LevelManagerScript.GetPlayerCount();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (_triggered) { return; }

        if(!AllPlayersInside()) { return; }

        foreach(GameObject en in _enemies) {
            en.SetActive(true);
        }
        _camera.LockCameraTo((Vector2)_lockPos.position);
        _triggered = true;
    }

}