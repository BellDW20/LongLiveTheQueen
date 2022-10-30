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
    [SerializeField] private Text[] pLevelText;
    [SerializeField] private Image[] pHeart;
    [SerializeField] private Image[] pAmmo;
    [SerializeField] private Text[] pAmmoText;
    [SerializeField] private Text[] pSpecialText;

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
            pLevelText[i].enabled = playerInGame;
            pHeart[i].enabled = playerInGame;
            pAmmo[i].enabled = playerInGame;
            pAmmoText[i].enabled = playerInGame;
            pSpecialText[i].enabled = playerInGame;
            if (playerInGame) {
                UpdatePlayerHealthVisual(i);
                UpdatePlayerScoreVisual(i);
                UpdatePlayerLevelVisual(i);
                UpdatePlayerStockVisual(i);
            }
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

    public static void UpdatePlayerLevelVisual(int player) {
        instance.pLevelText[player].text = "Dmg Level " + LevelManagerScript.pInfos[player].level;
    }

    public static void UpdatePlayerStockVisual(int player) {
        int stock = LevelManagerScript.pInfos[player].stock;
        instance.pStockText[player].text = (stock < 0) ? "Game Over" : ("P"+(player+1)+" - Stock: "+stock);
    }
    public static void UpdatePlayerSpecialVisual(int player, bool available) {
        instance.pSpecialText[player].enabled = available;
    }

    public static void UpdatePlayerAmmoVisual(int player, Gun gun) {
        Text text = instance.pAmmoText[player];
        if(gun.IsReloading()) {
            text.text = "RELOADING";
        } else {
            text.text = gun.GetBulletsInMag() + "/" + gun.GetMagSize();
        }
    }

}
