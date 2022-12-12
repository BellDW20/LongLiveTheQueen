using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour {

    [SerializeField] private GameObject[] _enemies;

    public void Start() {

        foreach (GameObject en in _enemies) {
            en.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        foreach(GameObject en in _enemies) {
            en.SetActive(true);
        }
        Destroy(gameObject);
    }

}