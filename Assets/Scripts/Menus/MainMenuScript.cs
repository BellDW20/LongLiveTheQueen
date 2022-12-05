using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _mainCanvas; //Canvas full of main menu UI objects
    [SerializeField] private GameObject _helpCanvas; //Canvas full of help menu UI objects
    [SerializeField] private GameObject _highscoreCanvas; //Canvas full of highscore menu UI objects

    public void Awake() {
        InputManager.ResetJoystickAssignment();
    }

    //When the multiplayer button is pressed on the main menu...
    public void OnArcadeModePressed() {
        SoundManager.PlaySFX(SFX.MENU_CONFIRM);
        LevelManagerScript.SetMode(GameMode.ARCADE_MODE);
        SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "CharacterSelectMenu");
    }

    public void OnHordeModePressed() {
        SoundManager.PlaySFX(SFX.MENU_CONFIRM);
        LevelManagerScript.SetMode(GameMode.HORDE_MODE);
        SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "CharacterSelectMenu");
    }

    //When the help button is pressed on the main menu...
    public void OnHelpPressed() {
        SoundManager.PlaySFX(SFX.MENU_CONFIRM);
        _mainCanvas.SetActive(false); //Make the main menu invisible
        _helpCanvas.SetActive(true); //Make the help menu visible
    }

    //When the quit button is pressed on the main menu...
    public void OnQuitPressed() {
        SoundManager.PlaySFX(SFX.MENU_CONFIRM);
        //Quit the game
        Application.Quit();
    }

    public void OnHighscorePressed() {
        SoundManager.PlaySFX(SFX.MENU_CONFIRM);
        _mainCanvas.SetActive(false);
        _highscoreCanvas.SetActive(true);
    }

}
