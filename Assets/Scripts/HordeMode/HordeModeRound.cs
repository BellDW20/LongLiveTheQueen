using UnityEngine;

public class HordeModeRound {

    public float ENEMY_HEALTH_SCALE;
    public float ENEMY_SPEED_SCALE;
    public int totalEnemies;

    private Horde[] _hordes;
    private float[] _hordeSpawnDelays;
    private int _hordeOn;

    private float _lastTimeHordeSpawned;

    private HordeModeRound(Horde[] hordes, float[] hordeSpawnDelays, float enemyHealthScale, float enemySpeedScale) {
        _hordes = hordes;
        _hordeSpawnDelays = hordeSpawnDelays;
        ENEMY_HEALTH_SCALE = enemyHealthScale;
        ENEMY_SPEED_SCALE = enemySpeedScale;

        foreach(Horde horde in hordes) {
            totalEnemies += horde.GetEnemyCount();
        }

        _lastTimeHordeSpawned = Time.time;
    }

    public void ScaleDifficulty(int scale) {
        ENEMY_HEALTH_SCALE *= scale;
        ENEMY_SPEED_SCALE *= Mathf.Sqrt(scale);
        foreach(Horde horde in _hordes) {
            horde.ScaleDifficulty(scale);
        }
    }

    public void TryToSpawnHorde() {
        if(_hordeOn < _hordeSpawnDelays.Length) {
            if (Time.time - _lastTimeHordeSpawned > _hordeSpawnDelays[_hordeOn]) {
                HordeModeMGR.SpawnHorde(_hordes[_hordeOn]);
                _hordeOn++;
                _lastTimeHordeSpawned = Time.time;
            }
        }
    }

    public bool IsFinishedSpawning() {
        return _hordeOn == _hordeSpawnDelays.Length;
    }

    public static HordeModeRound FromString(string str) {
        string[] hordeStrs = str.Split('\n');
        string[] difficultyParams = hordeStrs[0].Split(' ');
        Horde[] hordes = new Horde[hordeStrs.Length-1];
        float[] hordeSpawnDelays = new float[hordeStrs.Length-1];
        for (int i = 0; i < hordes.Length; i++) {
            string[] parts = hordeStrs[i+1].Split('>');
            hordes[i] = Horde.FromString(parts[0]);
            hordeSpawnDelays[i] = float.Parse(parts[1]);
        }
        return new HordeModeRound(hordes, hordeSpawnDelays, float.Parse(difficultyParams[0]), float.Parse(difficultyParams[1]));
    }

}
