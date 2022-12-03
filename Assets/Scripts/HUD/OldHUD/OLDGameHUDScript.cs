using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDScript : MonoBehaviour {

    private static GameHUDScript instance;

    [SerializeField] private GameObject[] playerHudObjects;
    [SerializeField] private GameObject bottomBanner;
    private PlayerHUDScript[] playerHuds;

    void Awake() {
        //make the current scene's HUD instance the current instance
        instance = this;
    }
    private void Start() {
        int players = 0;

        //For each player that could POSSIBLY be in the game...
        playerHuds = new PlayerHUDScript[playerHudObjects.Length];
        for (int i = 0; i < playerHudObjects.Length; i++) {
            //if that player is in the game...
            bool playerInGame = (LevelManagerScript.pInfos[i] != null);
            playerHudObjects[i].SetActive(playerInGame);
            if (playerInGame) {
                playerHuds[i] = playerHudObjects[i].GetComponent<PlayerHUDScript>();
                playerHuds[i].Refresh();
                players++;
            }
        }

        if(players > 2) {
            bottomBanner.SetActive(true);
        } else {
            bottomBanner.SetActive(false);
        }
    }

    //Updates the health bar for a specific player. Called only when
    //their health has changed to minimize useless work
    public static void UpdatePlayerHealthVisual(int player) {
        instance.playerHuds[player].UpdatePlayerHealthVisual();
    }

    //Updates the score text and XP bar for a specific player. Called only when
    //their score has changed to minimize useless work
    public static void UpdatePlayerScoreVisual(int player) {
        instance.playerHuds[player].UpdatePlayerScoreVisual();
    }

    //Updates the level text for a specific player. Called only when
    //their level has increased to minimize useless work
    public static void UpdatePlayerLevelVisual(int player) {
        instance.playerHuds[player].UpdatePlayerLevelVisual();
    }

    //Updates the stock text for a specific player. Called only when
    //they have lost a life to minimize useless work
    public static void UpdatePlayerStockVisual(int player) {
        instance.playerHuds[player].UpdatePlayerStockVisual();
    }

    //Updates the visibility of the special text for a specific player.
    public static void UpdatePlayerSpecialVisual(int player, bool available) {
        instance.playerHuds[player].UpdatePlayerSpecialVisual(available);
    }

    //Updates the gun visual (ammo and icon) for a specific player.
    public static void UpdatePlayerGunVisual(int player, Gun gun) {
        instance.playerHuds[player].UpdatePlayerGunVisual(gun);
    }

}
