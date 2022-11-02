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
            PlayerInfo pInfo = LevelManagerScript.pInfos[i];
            //If the player was in the game...
            //Show their score
            _pScoreText[i].enabled = (pInfo != null);
            if (pInfo != null) {
                _pScoreText[i].text = "P" + (i + 1) + " Score: " + pInfo.score;

                //Log their score to the high score manager
                int ranking = HighScoreManager.LogScore(pInfo.type, pInfo.score);
                //If they got a high score, make it known!
                if(ranking != -1) {
                    _pScoreText[i].text = _pScoreText[i].text+" (#"+ranking+" high score)";
                    _pScoreText[i].color = Color.green;
                }
            }
        }

        //If the result of the game was failure, show game over, and game clear otherwise
        _gameResultText.text = LevelManagerScript.WasGameWon() ? "Game Clear!" : "Game Over";
    }

    public void Update() {
        //If escape is typed, return to the main menu
        if(InputManager.GetBackInput(0) || InputManager.GetBackInput(1)) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "MainMenu");
        }
    }
}
