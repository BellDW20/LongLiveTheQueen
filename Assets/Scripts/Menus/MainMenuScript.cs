using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public void OnSinglePlayerPressed() {
        LevelManagerScript.SetupSinglePlayerGame(0);
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    public void OnCoOpPressed() {
        LevelManagerScript.SetupCoOpGame(0, 1);
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    public void OnHelpPressed() {
        SceneManager.LoadScene("HelpMenu");
    }

    public void OnQuitPressed() {
        Application.Quit();
    }

}
