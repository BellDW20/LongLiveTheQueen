using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _mainCanvas; //Canvas full of main menu UI objects
    [SerializeField] private GameObject _helpCanvas; //Canvas full of help menu UI objects
    [SerializeField] private GameObject _highscoreCanvas; //Canvas full of highscore menu UI objects

    //When the singleplayer button is pressed on the main menu...
    public void OnSinglePlayerPressed() {
        //Setup variables in the level manager for a singleplayer game
        LevelManagerScript.SetupSinglePlayerGame(0);
        //Then tell the level manager to start the first level using a full level transition
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    //When the multiplayer button is pressed on the main menu...
    public void OnCoOpPressed() {
        //Setup variables in the level manager for a multiplayer game
        LevelManagerScript.SetupCoOpGame(1, 0);
        //Then tell the level manager to start the first level using a full level transition
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    //When the help button is pressed on the main menu...
    public void OnHelpPressed() {
        _mainCanvas.SetActive(false); //Make the main menu invisible
        _helpCanvas.SetActive(true); //Make the help menu visible
    }

    //When the quit button is pressed on the main menu...
    public void OnQuitPressed() {
        //Quit the game
        Application.Quit();
    }

    public void OnHighscorePressed()
    {
        _mainCanvas.SetActive(false);
        _highscoreCanvas.SetActive(true);
    }

}
