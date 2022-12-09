using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeModeMGR : MonoBehaviour {

    private static HordeModeMGR instance;

    [SerializeField] private List<HordeModeArea> _areasUnlocked;
    [SerializeField] private GameObject[] _enemyPrefabs;

    private int _roundNumber, _playerCount;
    private HordeModeRound _currentRound;
    private int _enemiesCreated;
    private int _enemiesDestroyed;

    private bool _init;

    private float _lastTimeAmbientPlayed;

    //Unity Methods

    void Awake() {
        instance = this;
        _playerCount = LevelManagerScript.GetPlayerCount();
    }

    private void Start() {
        _init = false;
        _lastTimeAmbientPlayed = -64f;
    }

    private void Update() {
        if (!_init) {
            _roundNumber = 0;
            StartNextRound();
            _init = true;
        }

        if(Time.time - _lastTimeAmbientPlayed > 64f) {
            if(Random.value > 0.35f) {
                SoundManager.PlaySFX(SFX.AMBIENT_0);
            }
            _lastTimeAmbientPlayed = Time.time;
        }

        _currentRound.TryToSpawnHorde();
        NewGameHUD.EditBossBar("ENEMIES LEFT: ", 1.0f - (float)_enemiesDestroyed / (_playerCount * _currentRound.totalEnemies));
    }

    //Helper Methods

    private void StartNextRound() {
        _roundNumber++;

        int actualRound = ((_roundNumber-1)%10)+1;
        int roundScale = ((_roundNumber-1)/10)+1;
            
        _currentRound = HordeModeRound.FromString(
            Resources.Load<TextAsset>("HordeModeRounds/round"+actualRound).text
        );
        _currentRound.ScaleDifficulty(roundScale);
        _enemiesCreated = 0;
        _enemiesDestroyed = 0;

        NewGameHUD.UpdateRoundVisual(_roundNumber);
    }

    private Vector3 GetAvailableSpawnpoint() {
        int selectedArea = Random.Range(0, _areasUnlocked.Count);
        return _areasUnlocked[selectedArea].GetAvailableSpawnpoint();
    }

    private void CreateEnemy(GameObject prefab, Vector3 spawnpoint) {
        GameObject enemy = Instantiate(prefab, spawnpoint, Quaternion.identity);
        enemy.AddComponent<HordeEnemyScript>();
        enemy.GetComponent<EnemyHealthScript>().ScaleHealth(_currentRound.ENEMY_HEALTH_SCALE);

        Enemy pathingScript = enemy.GetComponent<Enemy>();
        if (pathingScript != null) {
            pathingScript.SPEED *= _currentRound.ENEMY_SPEED_SCALE;
            pathingScript.RANGE = 1000;
        }

        _enemiesCreated++;
    }

    private void i_SpawnEnemy(EnemyType enemy) {
        for (int i = 0; i < _playerCount; i++) {
            CreateEnemy(_enemyPrefabs[(int)enemy], GetAvailableSpawnpoint());
        }
    }

    private void i_SpawnHorde(EnemyType enemy, int count) {
        for (int p = 0; p < _playerCount; p++) {
            Vector3 spawnpoint = GetAvailableSpawnpoint();
            for (int i = 0; i < count; i++) {
                CreateEnemy(_enemyPrefabs[(int)enemy], spawnpoint);
            }
        }
    }

    private void i_SpawnHorde(Horde horde) {
        for (int p = 0; p < _playerCount; p++) {
            Vector3 spawnpoint = GetAvailableSpawnpoint();
            for (int i = 0; i < horde._enemyCounts.Length; i++) {
                GameObject enemyPrefab = _enemyPrefabs[(int)horde._enemyTypes[i]];
                for (int j = 0; j < horde._enemyCounts[i]; j++) {
                    CreateEnemy(enemyPrefab, spawnpoint);
                }
            }
        }
    }

    private void i_UnlockArea(HordeModeArea area) {
        _areasUnlocked.Add(area);
    }

    private void i_ReportEnemyDeath() {
        _enemiesDestroyed++;
        if(_currentRound.IsFinishedSpawning() && _enemiesCreated == _enemiesDestroyed) {
            StartNextRound();
            SoundManager.PlaySFX(SFX.ROUND_CLEAR);
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