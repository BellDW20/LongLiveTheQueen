using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDScript : MonoBehaviour {

    private static GameHUDScript instance;

    [SerializeField] private Text[] pStockText; //Texts for each player showing player lives
    [SerializeField] private Image[] pHealthBar; //Health bars for each player
    [SerializeField] private Text[] pScoreText; //Texts for each player showing player score
    [SerializeField] private Image[] pLevelBar; //XP bars for each player
    [SerializeField] private Text[] pLevelText; //Texts for each player showing player level
    [SerializeField] private Image[] pHeart; //Heart icons for each player's health bar
    [SerializeField] private Image[] pAmmo; //Ammo icons for each player=
    [SerializeField] private Text[] pAmmoText; //Texts for each player showing how much ammo they have
    [SerializeField] private Text[] pSpecialText; //Texts for each player showing if their special is available

    void Awake() {
        //make the current scene's HUD instance the current instance
        instance = this;
    }
    private void Start() {
        //For each player that could POSSIBLY be in the game...
        for (int i = 0; i < pStockText.Length; i++) {
            //if that player is in the game...
            bool playerInGame = (LevelManagerScript.pInfos[i] != null);
            //Make sure we show their HUD icons. If they aren't, don't show the HUD icons.
            pStockText[i].enabled = playerInGame;
            pHealthBar[i].enabled = playerInGame;
            pScoreText[i].enabled = playerInGame;
            pLevelBar[i].enabled = playerInGame;
            pLevelText[i].enabled = playerInGame;
            pHeart[i].enabled = playerInGame;
            pAmmo[i].enabled = playerInGame;
            pAmmoText[i].enabled = playerInGame;
            pSpecialText[i].enabled = playerInGame;

            //If the player is in the game, make sure their HUD visuals are up to date
            // (we want to carry over values, not use default values)
            if (playerInGame) {
                UpdatePlayerHealthVisual(i);
                UpdatePlayerScoreVisual(i);
                UpdatePlayerLevelVisual(i);
                UpdatePlayerStockVisual(i);
            }
        }
    }

    //Updates the health bar for a specific player. Called only when
    //their health has changed to minimize useless work
    public static void UpdatePlayerHealthVisual(int player) {
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];

        //scale the health bar by the ratio of health to max health of the player
        Vector3 scale = new Vector3(Mathf.Clamp(pInfo.health / pInfo.GetMaxHealth(), 0, 1), 1, 1);
        instance.pHealthBar[player].transform.localScale = scale;
    }

    //Updates the score text and XP bar for a specific player. Called only when
    //their score has changed to minimize useless work
    public static void UpdatePlayerScoreVisual(int player) {
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];

        //Sets the score text to the most up to date value
        instance.pScoreText[player].text = "Score: " + pInfo.score;

        //Scales the player's XP bar based on their progress to the next level
        Vector3 scale = new Vector3(pInfo.ProgressToNextLevel(), 1, 1);
        instance.pLevelBar[player].transform.localScale = scale;
    }

    //Updates the level text for a specific player. Called only when
    //their level has increased to minimize useless work
    public static void UpdatePlayerLevelVisual(int player) {
        //Sets the level text to the most up to date level
        instance.pLevelText[player].text = "Dmg Level " + LevelManagerScript.pInfos[player].level;
    }

    //Updates the stock text for a specific player. Called only when
    //they have lost a life to minimize useless work
    public static void UpdatePlayerStockVisual(int player) {
        int stock = LevelManagerScript.pInfos[player].stock;
        
        //Set the stock text to the remaining stocks, or to "Game Over" if
        //the player has no stocks left to use
        instance.pStockText[player].text = (stock < 0) ? "Game Over" : ("P"+(player+1)+" - Stock: "+stock);
    }

    //Updates the visibility of the special text for a specific player.
    public static void UpdatePlayerSpecialVisual(int player, bool available) {
        //Makes the special indicator visible if the player can use their special, and invisible if not
        instance.pSpecialText[player].enabled = available;
    }

    //Updates the ammo text for a specific player.
    public static void UpdatePlayerAmmoVisual(int player, Gun gun) {
        Text text = instance.pAmmoText[player];

        if(gun.IsReloading()) {
            //If the player is reloading set the text to indicate that
            text.text = "RELOADING";
        } else {
            //Otherwise, set the text to show the bullets left and magazine size
            text.text = gun.GetBulletsInMag() + "/" + gun.GetMagSize();
        }
    }

}
