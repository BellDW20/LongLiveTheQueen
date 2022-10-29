using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _helpCanvas;

    public void OnSinglePlayerPressed() {
        LevelManagerScript.SetupSinglePlayerGame(0);
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    public void OnCoOpPressed() {
        LevelManagerScript.SetupCoOpGame(0, 1);
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    public void OnHelpPressed() {
        //SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "HelpMenu
        _mainCanvas.SetActive(false);
        _helpCanvas.SetActive(true);
    }

    public void OnQuitPressed() {
        Application.Quit();
    }

}
