using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeModeMGR : MonoBehaviour {

    private static HordeModeMGR instance;

    [SerializeField] private List<HordeModeArea> _areasUnlocked;
    [SerializeField] private GameObject[] _enemyPrefabs;

    private int _roundNumber;
    private HordeModeRound _currentRound;
    private int _enemiesCreated;
    private int _enemiesDestroyed;

    private bool _init;

    //Unity Methods

    void Awake() {
        instance = this;
    }

    private void Start() {
        _init = false;
    }

    private void Update() {
        if (!_init) {
            _roundNumber = 0;
            StartNextRound();
            NewGameHUD.SetAsHordeMode();
            _init = true;
        }

        _currentRound.TryToSpawnHorde();
    }

    //Helper Methods

    private void StartNextRound() {
        _roundNumber++;
        _currentRound = HordeModeRound.FromString(
            Resources.Load<TextAsset>("HordeModeRounds/round"+_roundNumber).text
        );
        _enemiesCreated = 0;
        _enemiesDestroyed = 0;
    }

    private Vector3 GetAvailableSpawnpoint() {
        int selectedArea = Random.Range(0, _areasUnlocked.Count);
        return _areasUnlocked[selectedArea].GetAvailableSpawnpoint();
    }

    private void CreateEnemy(GameObject prefab, Vector3 spawnpoint) {
        GameObject enemy = Instantiate(prefab, spawnpoint, Quaternion.identity);
        enemy.AddComponent<HordeEnemyScript>();
        _enemiesCreated++;
    }

    private void i_SpawnEnemy(EnemyType enemy) {
        CreateEnemy(_enemyPrefabs[(int)enemy], GetAvailableSpawnpoint());
    }

    private void i_SpawnHorde(EnemyType enemy, int count) {
        Vector3 spawnpoint = GetAvailableSpawnpoint();
        for(int i=0; i<count; i++) {
            CreateEnemy(_enemyPrefabs[(int)enemy], spawnpoint);
        }
    }

    private void i_SpawnHorde(Horde horde) {
        Vector3 spawnpoint = GetAvailableSpawnpoint();
        for(int i=0; i<horde._enemyCounts.Length; i++) {
            GameObject enemyPrefab = _enemyPrefabs[(int)horde._enemyTypes[i]];
            for (int j=0; j<horde._enemyCounts[i]; j++) {
                CreateEnemy(enemyPrefab, spawnpoint);
            }
        }
    }

    private void i_UnlockArea(HordeModeArea area) {
        _areasUnlocked.Add(area);
    }

    private void i_ReportEnemyDeath() {
        _enemiesDestroyed++;
        if(_currentRound.IsFinishedSpawning() && _enemiesCreated == _enemiesDestroyed) {
            print("Round Over!");
            StartNextRound();
        }
    }

    //Static wrappers
    public static void SpawnEnemy(EnemyType enemy) {
        instance.i_SpawnEnemy(enemy);
    }

    public static void SpawnHorde(EnemyType enemy, int count) {
        instance.i_SpawnHorde(enemy, count);
    }

    public static void SpawnHorde(Horde horde) {
        instance.i_SpawnHorde(horde);
    }

    public static void UnlockArea(HordeModeArea area) {
        instance.i_UnlockArea(area);
    }
    public static void ReportEnemyDeath() {
        instance.i_ReportEnemyDeath();
    }

}