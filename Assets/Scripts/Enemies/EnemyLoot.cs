using UnityEngine;

[System.Serializable]
public class EnemyLoot {

    [SerializeField] private float _lootChance;

    public void GenerateLoot(Vector3 position) {
        if(LevelManagerScript.GetMode() == GameMode.HORDE_MODE || _lootChance == 0) { return; }
        TryToGenerateLoot(position);
    }

    private void TryToGenerateLoot(Vector3 position) {
        if (Random.value > _lootChance) { return; }

        float toSpawn = Random.value;
        if(toSpawn < 0.5) {
            Object.Instantiate(LevelManagerScript.GetGun(GunType.SHOTGUN), position, Quaternion.identity);
        } else if (toSpawn < 0.85) {
             Object.Instantiate(LevelManagerScript.GetGun(GunType.LMG), position, Quaternion.identity);
        } else if (toSpawn < 0.95) {
            Object.Instantiate(LevelManagerScript.GetGun(GunType.RPG), position, Quaternion.identity);
        } else {
            Object.Instantiate(LevelManagerScript.GetGun(GunType.BFG), position, Quaternion.identity);
        }
    }


}