using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameHUD : MonoBehaviour {

    private static NewGameHUD instance;

    [SerializeField] private GameObject[] playerHudObjects;
    [SerializeField] private Image[] playerHudBackdrops;
    private NewPlayerHUD[] playerHuds;

    [SerializeField] private GameObject bossHud;
    [SerializeField] private Transform bossHealthBar;
    private EnemyHealthScript bossHealthScript;
    private bool isBossActive;

    [SerializeField] GameObject timerHud;
    [SerializeField] Text timerText;

    void Awake() {
        //make the current scene's HUD instance the current instance
        instance = this;
    }

    private void Start() {
        int players = 0;

        //For each player that could POSSIBLY be in the game...
        playerHuds = new NewPlayerHUD[playerHudObjects.Length];
        for (int i = 0; i < playerHudObjects.Length; i++) {
            //if that player is in the game...
            bool playerInGame = (LevelManagerScript.pInfos[i] != null);
            playerHudObjects[i].SetActive(playerInGame);
            playerHudBackdrops[i].enabled = playerInGame;
            if (playerInGame) {
                playerHuds[i] = playerHudObjects[i].GetComponent<NewPlayerHUD>();
                playerHuds[i].Refresh();
                players++;
            }
        }
    }

    public void Update() {
        if(isBossActive) { UpdateBossHealthBar(); }

        float time = LevelManagerScript.GetTimeTakenOnLevel();
        int minutes = (int)Mathf.Floor(time / 60);
        time -= 60 * minutes;
        int seconds = (int)time;

        timerText.text = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
    }

    private void UpdateBossHealthBar() {
        if(bossHealthScript == null) {
            isBossActive = false;
            bossHud.SetActive(false);
            return;
        }
        bossHealthBar.localScale = Vector3.Lerp(
            bossHealthBar.localScale, 
            new Vector3(Mathf.Clamp01(bossHealthScript.GetHealth() / bossHealthScript.GetMaxHealth()), 1, 1),
            0.5f
        );
    }

    private void i_BeginBossFight(EnemyHealthScript boss) {
        bossHealthScript = boss;
        isBossActive = true;
        bossHud.SetActive(true);
        bossHealthBar.localScale = new Vector3(1, 1, 1);
    }

    public static void SetAsHordeMode() {
        instance.timerHud.SetActive(false);
        for(int i=0; i<LevelManagerScript.GetPlayerCount(); i++) {
            instance.playerHuds[i].SetHordeMode(true);
            instance.playerHuds[i].Refresh();
        }
    }

    //Updates the health bar for a specific player. Called only when
    //their health has changed to minimize useless work
    public static void UpdatePlayerHealthVisual(int player) {
        instance.playerHuds[player].UpdateHealthVisual();
    }

    //Updates the score text and XP bar for a specific player. Called only when
    //their score has changed to minimize useless work
    public static void UpdatePlayerScoreVisual(int player) {
        instance.playerHuds[player].UpdateScoreVisual();
    }

    //Updates the stock text for a specific player. Called only when
    //they have lost a life to minimize useless work
    public static void UpdatePlayerStockVisual(int player) {
        instance.playerHuds[player].UpdateStockVisual();
    }

    //Updates the gun visual (ammo and icon) for a specific player.
    public static void UpdatePlayerGunVisual(int player, Gun gun) {
        instance.playerHuds[player].UpdateGunVisual(gun);
    }

    public static void UpdatePlayerSpecialVisual(int player, Gun special) {
        instance.playerHuds[player].UpdateSpecialVisual(special);
    }

    public static void BeginBossFight(EnemyHealthScript boss) {
        instance.i_BeginBossFight(boss);
    }

}
