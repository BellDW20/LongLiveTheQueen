using UnityEngine;

public class StagedFightArea : MonoBehaviour {

    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private CameraScript _camera;
    [SerializeField] private Transform _lockPos;

    private bool _triggered;

    public void Start() {
        _triggered = false;
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

    public void OnTriggerEnter2D(Collider2D collision) {
        if(_triggered) { return; }
        foreach(GameObject en in _enemies) {
            en.SetActive(true);
        }
        _camera.LockCameraTo((Vector2)_lockPos.position);
        _triggered = true;
    }

}