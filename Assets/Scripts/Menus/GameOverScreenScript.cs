using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreenScript : MonoBehaviour {

    [SerializeField] private Text _gameResultText;
    [SerializeField] private Text[] _pScoreText;

    void Start() {
        for(int i=0; i<_pScoreText.Length; i++) {
            bool playerInGame = (LevelManagerScript.pInfos[i] != null);
            _pScoreText[i].enabled = playerInGame;
            if(playerInGame) {
                _pScoreText[i].text = "P" + (i + 1) + " Score: " + LevelManagerScript.pInfos[i].score;
                //TODO: flash if high score
            }
        }
        _gameResultText.text = LevelManagerScript.WasGameWon() ? "Game Clear!" : "Game Over";
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "MainMenu");
        }
    }
}
