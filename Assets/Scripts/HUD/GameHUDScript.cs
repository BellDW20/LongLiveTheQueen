using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDScript : MonoBehaviour {

    private static GameHUDScript instance;

    [SerializeField] private Text[] pStockText;
    [SerializeField] private Image[] pHealthBar;
    [SerializeField] private Text[] pScoreText;
    [SerializeField] private Image[] pLevelBar;

    void Awake() {
        instance = this;
    }
    private void Start() {
        for (int i = 0; i < pStockText.Length; i++) {
            bool playerInGame = (LevelManagerScript.pInfos[i] != null);
            pStockText[i].enabled = playerInGame;
            pHealthBar[i].enabled = playerInGame;
            pScoreText[i].enabled = playerInGame;
            pLevelBar[i].enabled = playerInGame;
        }
    }

    public static void UpdatePlayerHealthVisual(int player) {
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];
        Vector3 scale = new Vector3(Mathf.Clamp(pInfo.health / pInfo.GetMaxHealth(), 0, 1), 1, 1);
        instance.pHealthBar[player].transform.localScale = scale;
    }

    public static void UpdatePlayerScoreVisual(int player) {
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];
        instance.pScoreText[player].text = "Score: " + pInfo.score;
        Vector3 scale = new Vector3(pInfo.ProgressToNextLevel(), 1, 1);
        instance.pLevelBar[player].transform.localScale = scale;
    }

    public static void UpdatePlayerStockVisual(int player) {
        instance.pStockText[player].text = "P"+(player+1)+" - Stock: "+LevelManagerScript.pInfos[player].stock;
    }

}
