using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDScript : MonoBehaviour {

    private static GameHUDScript instance;

    [SerializeField] private Text p1Text;
    [SerializeField] private Text p2Text;
    [SerializeField] private Image p1Health;
    [SerializeField] private Image p2Health;

    void Awake() {
        instance = this;
    }
    private void Start() {
        bool p1InGame = (LevelManagerScript.pInfos[0] != null);
        bool p2InGame = (LevelManagerScript.pInfos[1] != null);
        p1Text.enabled = p1InGame;
        p2Text.enabled = p2InGame;
        p1Health.enabled = p1InGame;
        p2Health.enabled = p2InGame;
    }
    public static void UpdatePlayerHealthVisual(int player) {
        PlayerInfo pInfo = LevelManagerScript.pInfos[player];
        Vector3 scale = new Vector3(Mathf.Clamp(pInfo.health / pInfo.GetMaxHealth(), 0, 1), 1, 1);
        if(player == 0) {
            instance.p1Health.transform.localScale = scale;
        } else if(player == 1) {
            instance.p2Health.transform.localScale = scale;
        }
    }

}
