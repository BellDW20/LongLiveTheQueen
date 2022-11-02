using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour {

    //Level codes representing levels / sub levels [FORMAT: LEVEL_{level number}_{sublevel number}]
    public const int LEVEL_1_1 = 0;
    public const int LEVEL_2_1 = 1;
    public const int LEVEL_3_1 = 2;

    //Names of the level/sublevel scenes corresponding to level codes
    private static readonly string[] LEVEL_SCENE_NAMES = {
        "Level1Scene", "Level2Scene", "BossRoom"
    };
    //Names of the levels/sublevels to be displayed corresponding to level codes
    private static readonly string[] LEVEL_DISPLAY_NAMES = {
        "Level 1", "Level 2", "Final Boss"
    };

    //Constants representing how to transition between level scenes
    public const int SUBLEVEL_TRANSITION = 0; //Transition merely fades b/t scenes, maintaining previous player stats
    public const int LEVEL_TRANSITION = 1; //Transition fades b/t scenes and resets health / ammo for next level

    private static int _levelToTransitionTo; //Level to transition to
    private static int _levelTransitionType; //How the level should be transitioned to (constants above are used)

    [SerializeField] private Transform[] _pSpawns; //The positions at which players spawn
    [SerializeField] private GameObject[] _PlayerPrefabs; //The prefabs for all types of players
    private static GameObject[] players = new GameObject[2]; //References to each player in the active level
    public static PlayerInfo[] pInfos = new PlayerInfo[2]; //The data for each player saved globally to carry across levels
    private static bool _gameWon; //Whether or not the game was won upon the game ending (false=game over)

    void Start() {
        //for every POSSIBLE player (P1, P2, etc.)
        for (int i = 0; i < pInfos.Length; i++) {
            //If that player is actually in the game
            if (pInfos[i] != null) {
                //If we are doing a full level transition
                if (_levelTransitionType == LEVEL_TRANSITION) {
                    //Reset important player info like health/ammo
                    pInfos[i].ClearForNextLevel();
                }

                //Create a player of the correct type (sniper, commando, etc.)
                //at that given player number's spawn point in the current level
                players[i] = Instantiate(
                    _PlayerPrefabs[pInfos[i].type],
                    new Vector2(_pSpawns[i].position.x, _pSpawns[i].position.y),
                    Quaternion.identity
                );
            }
        }
    }

    //Sets up a transition to a given level / sublevel
    public static void BeginLevel(int levelID, int transitionType) {
        _levelTransitionType = transitionType;
        _levelToTransitionTo = levelID;
        SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "LevelTransitionScene");
    }

    //Sets up for a singleplayer game
    public static void SetupSinglePlayerGame(int p1Type) {
        //Make sure to mark the newest game as not yet won
        _gameWon = false;
        //The first player is in the game, using the chosen character (p1Type)
        pInfos[0] = new PlayerInfo(p1Type);
        //No other players are in the game
        pInfos[1] = null;
    }

    //Sets up for a multiplayer game
    public static void SetupCoOpGame(int p1Type, int p2Type) {
        //Make sure to mark the newest game as not yet won
        _gameWon = false;
        //The first player is in the game, using the chosen character (p1Type)
        pInfos[0] = new PlayerInfo(p1Type);
        //The first player is in the game, using the chosen character (p2Type)
        pInfos[1] = new PlayerInfo(p2Type);
    }

    //Uses the saved references to tell a player object
    //their player number
    public static int GetPlayerNumber(GameObject player) {
        //For each player reference we have
        for (int i = 0; i < pInfos.Length; i++) {
            //If the given player object matches that reference, then they are player number i
            if (Object.ReferenceEquals(players[i], player)) { return i; }
        }
        //Otherwise, the object passed was not a player, and is given an invalid player number
        return -1;
    }

    //Determines if the game is over
    public static bool IsGameOver() {
        int playersInGame = 0;
        int playersDead = 0;
        //Calculate the number of players in the game total,
        //as well as how many of them have used up all of their continues
        for (int i = 0; i < pInfos.Length; i++) {
            if(pInfos[i] != null) {
                playersInGame++;
                if(pInfos[i].stock < 0) { playersDead++; }
            }
        }
        //If the number of players in the game total
        //matches the number who have used up all of their continues,
        //a game over has occurred
        return (playersInGame == playersDead);
    }

    public static void WinGame() {
        _gameWon = true;
    }

    public static bool WasGameWon() {
        return _gameWon;
    }

    //Gives the display name of the currently loaded level
    public static string GetLevelName() {
        return LEVEL_DISPLAY_NAMES[_levelToTransitionTo];
    }

    //Gives the scene name of the currently loaded level
    public static string GetLevelSceneName() {
        return LEVEL_SCENE_NAMES[_levelToTransitionTo];
    }

}
