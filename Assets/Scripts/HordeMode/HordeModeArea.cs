using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeModeArea : MonoBehaviour {

    [SerializeField] private Transform[] _spawnPoints;

    public Vector3 GetAvailableSpawnpoint() {
        int selectedSpawnpoint = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[selectedSpawnpoint].position;
    }

}
