using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDScript : MonoBehaviour {

    [SerializeField] private Text pStockText; //Text showing player lives
    [SerializeField] private Image pHealthBar; //Health bar for player
    [SerializeField] private Text pScoreText; //Text showing player score
    [SerializeField] private Image pLevelBar; //XP bar
    [SerializeField] private Text pLevelText; //Text showing player level
    [SerializeField] private Text pAmmoText; //Text showing how much ammo the player has
    [SerializeField] private Text pSpecialText; //Text showing if special is available
    [SerializeField] private int playerNum;

    public void Start() {
        pStockText.color = PlayerInfo.PLAYER_NUM_COLORS[playerNum];
    }

    public void Refresh() {
        UpdatePlayerHealthVisual();
        UpdatePlayerScoreVisual();
        UpdatePlayerLevelVisual();
        UpdatePlayerStockVisual();
    }

    //Updates the health bar for a specific player. Called only when
    //their health has changed to minimize useless work
    public void UpdatePlayerHealthVisual() {
        PlayerInfo pInfo = LevelManagerScript.pInfos[playerNum];

        //scale the health bar by the ratio of health to max health of the player
        Vector3 scale = new Vector3(Mathf.Clamp(pInfo.health / pInfo.GetMaxHealth(), 0, 1), 1, 1);
        pHealthBar.transform.localScale = scale;
    }

    //Updates the score text and XP bar for a specific player. Called only when
    //their score has changed to minimize useless work
    public void UpdatePlayerScoreVisual() {
        PlayerInfo pInfo = LevelManagerScript.pInfos[playerNum];

        //Sets the score text to the most up to date value
        pScoreText.text = "Score: " + pInfo.score;

        //Scales the player's XP bar based on their progress to the next level
        Vector3 scale = new Vector3(pInfo.ProgressToNextLevel(), 1, 1);
        pLevelBar.transform.localScale = scale;
    }

    //Updates the level text for a specific player. Called only when
    //their level has increased to minimize useless work
    public void UpdatePlayerLevelVisual() {
        //Sets the level text to the most up to date level
        pLevelText.text = "Dmg Level " + LevelManagerScript.pInfos[playerNum].level;
    }

    //Updates the stock text for a specific player. Called only when
    //they have lost a life to minimize useless work
    public void UpdatePlayerStockVisual() {
        int stock = LevelManagerScript.pInfos[playerNum].stock;

        //Set the stock text to the remaining stocks, or to "Game Over" if
        //the player has no stocks left to use
        pStockText.text = (stock < 0) ? "Game Over" : ("P" + (playerNum + 1) + " - Stock: " + stock);
    }

    //Updates the visibility of the special text for a specific player.
    public void UpdatePlayerSpecialVisual(bool available) {
        //Makes the special indicator visible if the player can use their special, and invisible if not
        pSpecialText.enabled = available;
    }

    //Updates the ammo text for a specific player.
    public void UpdatePlayerAmmoVisual(Gun gun) {
        if (gun.IsReloading()) {
            //If the player is reloading set the text to indicate that
            pAmmoText.text = "RELOADING";
        } else {
            //Otherwise, set the text to show the bullets left and magazine size
            pAmmoText.text = gun.GetBulletsInMag() + "/" + gun.GetMagSize();
        }
    }

}
