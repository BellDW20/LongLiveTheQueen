using UnityEngine;
public class HordeEnemyScript : MonoBehaviour {

    public void OnDestroy() {
        HordeModeMGR.ReportEnemyDeath();
    }

}
