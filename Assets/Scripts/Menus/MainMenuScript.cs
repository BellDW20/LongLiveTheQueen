using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    public void OnSinglePlayerPressed() {
        LevelManagerScript.SetupSinglePlayerGame(1);
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    public void OnCoOpPressed() {
        LevelManagerScript.SetupCoOpGame(0, 1);
        LevelManagerScript.BeginLevel(LevelManagerScript.LEVEL_1_1, LevelManagerScript.LEVEL_TRANSITION);
    }

    public void OnQuitPressed() {
        Application.Quit();
    }

}
