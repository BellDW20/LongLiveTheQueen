using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour {

    public const int LEVEL_1_1 = 0;
    public const int LEVEL_2_1 = 1;
    private static readonly string[] LEVEL_SCENE_NAMES = {
        "Level1Scene", "Level2Scene"
    };
    private static readonly string[] LEVEL_DISPLAY_NAMES = {
        "Level 1", "Level 2"
    };
    public const int SUBLEVEL_TRANSITION = 0;
    public const int LEVEL_TRANSITION = 1;

    private static int _levelToTransitionTo;
    private static int _levelTransitionType;

    [SerializeField] private Transform[] _pSpawns;
    [SerializeField] private GameObject[] _PlayerPrefabs;
    private static GameObject[] players = new GameObject[2];
    public static PlayerInfo[] pInfos = new PlayerInfo[2];
    private static bool _gameWon;

    void Start() {
        if (_levelTransitionType == LEVEL_TRANSITION) {
            //reset important player info for next level
            if (pInfos[0] != null) { pInfos[0].ClearForNextLevel(); }
            if (pInfos[1] != null) { pInfos[1].ClearForNextLevel(); }
        }

        for (int i = 0; i < pInfos.Length; i++) {
            if (pInfos[i] != null) {
                if (_levelTransitionType == LEVEL_TRANSITION) {
                    pInfos[i].ClearForNextLevel();
                }
                players[i] = Instantiate(
                    _PlayerPrefabs[pInfos[i].type],
                    new Vector2(_pSpawns[i].position.x, _pSpawns[i].position.y),
                    Quaternion.identity
                );
            }
        }
    }

    public static void BeginLevel(int levelID, int transitionType) {
        _levelTransitionType = transitionType;
        _levelToTransitionTo = levelID;
        SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "LevelTransitionScene");
    }

    public static void SetupSinglePlayerGame(int p1Type) {
        pInfos[0] = new PlayerInfo(p1Type);
        pInfos[1] = null;
    }

    public static void SetupCoOpGame(int p1Type, int p2Type) {
        pInfos[0] = new PlayerInfo(p1Type);
        pInfos[1] = new PlayerInfo(p2Type);
    }

    public static int GetPlayerNumber(GameObject player) {
        for (int i = 0; i < pInfos.Length; i++) {
            if (Object.ReferenceEquals(players[i], player)) { return i; }
        }
        return -1;
    }

    public static bool IsGameOver() {
        int playersInGame = 0;
        int playersDead = 0;
        for (int i = 0; i < pInfos.Length; i++) {
            if(pInfos[i] != null) {
                playersInGame++;
                if(pInfos[i].stock < 0) { playersDead++; }
            }
        }
        return (playersInGame == playersDead);
    }

    public static bool WasGameWon() {
        return _gameWon;
    }

    public static string GetLevelName() {
        return LEVEL_DISPLAY_NAMES[_levelToTransitionTo];
    }

    public static string GetLevelSceneName() {
        return LEVEL_SCENE_NAMES[_levelToTransitionTo];
    }

}
