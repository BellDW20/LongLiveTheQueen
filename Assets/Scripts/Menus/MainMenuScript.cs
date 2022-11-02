using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _mainCanvas; //Canvas full of main menu UI objects
    [SerializeField] private GameObject _helpCanvas; //Canvas full of help menu UI objects
    [SerializeField] private GameObject _highscoreCanvas; //Canvas full of highscore menu UI objects

    [SerializeField] private RectTransform _sky0Transform; //The two sky backgrounds to scroll
    [SerializeField] private RectTransform _sky1Transform;
    [SerializeField] private RectTransform _castle0Transform; //The two castle backgrounds to scroll
    [SerializeField] private RectTransform _castle1Transform;

    private void Update() {
        //IGNORE THE MESS :)

        //We move both sky backgrounds to the left...
        _sky0Transform.position = new Vector3(_sky0Transform.position.x - 400 * Time.deltaTime, _sky0Transform.position.y, _sky0Transform.position.z);
        _sky1Transform.position = new Vector3(_sky1Transform.position.x - 400 * Time.deltaTime, _sky1Transform.position.y, _sky1Transform.position.z);
        //and if the one goes too far...
        if(_sky0Transform.position.x < -640) {
            //wrap it back around so it looks like the sky is scrolling seamlessly
            _sky0Transform.position = new Vector3(_sky0Transform.position.x + 2240, _sky0Transform.position.y, _sky0Transform.position.z);
            RectTransform temp = _sky1Transform;
            _sky1Transform = _sky0Transform;
            _sky0Transform = temp;
        }

        //Perform the same logic as above for the castle background but at a slower speed
        _castle0Transform.position = new Vector3(_castle0Transform.position.x - 250 * Time.deltaTime, _castle0Transform.position.y, _castle0Transform.position.z);
        _castle1Transform.position = new Vector3(_castle1Transform.position.x - 250 * Time.deltaTime, _castle1Transform.position.y, _castle1Transform.position.z);
        if (_castle0Transform.position.x < -640) {
            _castle0Transform.position = new Vector3(_castle0Transform.position.x + 2240, _castle0Transform.position.y, _castle0Transform.position.z);
            RectTransform temp = _castle1Transform;
            _castle1Transform = _castle0Transform;
            _castle0Transform = temp;
        }
    }

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
