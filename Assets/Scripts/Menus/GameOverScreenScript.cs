using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverScreenScript : MonoBehaviour {

    [SerializeField] private Text _gameResultText; //Text showing either a win or a game over
    [SerializeField] private Text[] _pScoreText; //Texts displaying the active players scores in game
    [SerializeField] private Text _pTimeText; //Text displaying how long the players took

    void Start() {

        List<int> playerTypes = new List<int>();
        //For every POSSIBLE player
        for(int i=0; i<_pScoreText.Length; i++) {
            PlayerInfo pInfo = LevelManagerScript.pInfos[i];
            //If the player was in the game...
            _pScoreText[i].enabled = (pInfo != null);
            if (pInfo != null) {

                //Show their score
                _pScoreText[i].text = "P" + (i + 1) + " Score: " + pInfo.score;

                //Add to list of active player types
                playerTypes.Add(pInfo.type);

                //Log their score to the high score manager
                int ranking = HighScoreManager.LogScore(pInfo.type, pInfo.score, 0);

                //If they got a high score, make it known!
                if(ranking != -1) {
                    _pScoreText[i].text = _pScoreText[i].text+" (#"+ranking+" high score)";
                    _pScoreText[i].color = Color.green;
                }
            }
        }

        //If the game was not won then we need to add the additional time left over before the level was completed
        if (!LevelManagerScript.WasGameWon())
        {
            LevelGoalScript.SetGameTime(LevelManagerScript.GetTimeTakenOnLevel());
        }

        //Log the group's time to the high score manager
        TimeSpan time = TimeSpan.FromSeconds((double)new decimal(LevelGoalScript.GetGameTime()));
        //Only want to log if game was won
        int timeRanking = LevelManagerScript.WasGameWon() ? HighScoreManager.LogTime(playerTypes, time, 0) : -1;

        //Display time taken and rank (if applicable)
        //float timeTaken = LevelGoalScript.GetGameTime();
        //int minutes = (int)Mathf.Floor(timeTaken / 60);
        //timeTaken -= 60 * minutes;
        //int seconds = (int)timeTaken;
        //_pTimeText.text = "Time: " + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
        _pTimeText.text = "Time: " + time.ToString("mm':'ss");

        if (timeRanking != -1)
        {
            _pTimeText.text = _pTimeText.text + " (#" + timeRanking + " time)";
            _pTimeText.color = Color.green;
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
