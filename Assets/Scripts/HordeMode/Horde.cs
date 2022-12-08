
public class Horde {

    public readonly EnemyType[] _enemyTypes;
    public int[] _enemyCounts;

    public Horde(EnemyType[] enemyTypes, int[] enemyCounts) {
        _enemyTypes = enemyTypes;
        _enemyCounts = enemyCounts;
    }

    public void ScaleDifficulty(int scale) {
        for (int i = 0; i < _enemyCounts.Length; i++) {
            _enemyCounts[i] += (scale - 1);
        }
    }

    public static Horde FromString(string str) {
        string[] parts = str.Split(' ');

        EnemyType[] enemyTypes = new EnemyType[parts.Length];
        int[] enemyCounts = new int[parts.Length];

        for(int i=0; i<parts.Length; i++) {
            string[] subParts = parts[i].Split(':');
            enemyTypes[i] = TypeFromString(subParts[0]);
            enemyCounts[i] = int.Parse(subParts[1]);
        }

        return new Horde(enemyTypes, enemyCounts);
    }

    private static EnemyType TypeFromString(string str) {
        switch (str) {
            case "LIGHT_ENEMY":
                return EnemyType.LIGHT_ENEMY;
            case "HEAVY_ENEMY":
                return EnemyType.HEAVY_ENEMY;
            case "BOMBER":
                return EnemyType.BOMBER;
            case "KNIGHT":
                return EnemyType.KNIGHT;
            case "MAGE":
                return EnemyType.MAGE;
            case "MINIBOSS":
                return EnemyType.MINIBOSS;
            case "QUEEN":
                return EnemyType.QUEEN;
            default:
                return EnemyType.LIGHT_ENEMY;
        }
    }

}