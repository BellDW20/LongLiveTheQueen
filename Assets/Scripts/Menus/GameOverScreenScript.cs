using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreenScript : MonoBehaviour {

    [SerializeField] private Text _gameResultText; //Text showing either a win or a game over
    [SerializeField] private Text[] _pScoreText; //Texts displaying the active players scores in game

    void Start() {
        //For every POSSIBLE player
        for(int i=0; i<_pScoreText.Length; i++) {
            //If the player was in the game...
            bool playerInGame = (LevelManagerScript.pInfos[i] != null);
            //Show their score
            _pScoreText[i].enabled = playerInGame;
            if(playerInGame) {
                _pScoreText[i].text = "P" + (i + 1) + " Score: " + LevelManagerScript.pInfos[i].score;
                //TODO: flash if high score
            }
        }

        //If the result of the game was failure, show game over, and game clear otherwise
        _gameResultText.text = LevelManagerScript.WasGameWon() ? "Game Clear!" : "Game Over";
    }

    public void Update() {
        //If escape is typed, return to the main menu
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "MainMenu");
        }
    }
}
