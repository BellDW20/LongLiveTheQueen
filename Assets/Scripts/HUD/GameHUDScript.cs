using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDScript : MonoBehaviour {

    private static GameHUDScript instance;

    [SerializeField] private Text[] pStockText;
    [SerializeField] private Image[] pHealthBar;
    [SerializeField] private Text[] pScoreText;

    void Awake() {
        instance = this;
    }
    private void Start() {
        for (int i = 0; i < pStockText.Length; i++) {
            bool playerInGame = (LevelManagerScript.pInfos[i] != null);
            pStockText[i].enabled = playerInGame;
            pHealthBar[i].enabled = playerInGame;
            pScoreText[i].enabled = playerInGame;
        }
    }

    public static void UpdatePlayerHealthVisual(int player) {
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];
        Vector3 scale = new Vector3(Mathf.Clamp(pInfo.health / pInfo.GetMaxHealth(), 0, 1), 1, 1);
        instance.pHealthBar[player].transform.localScale = scale;
    }

    public static void UpdatePlayerScoreVisual(int player) {
        instance.pScoreText[player].text = "Score: " + LevelManagerScript.pInfos[player].score;
    }

    public static void UpdatePlayerStockVisual(int player) {
        instance.pStockText[player].text = "P"+(player+1)+" - Stock: "+LevelManagerScript.pInfos[player].stock;
    }

}
