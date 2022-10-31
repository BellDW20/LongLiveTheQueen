using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Used between full levels to show which level is being played
public class LevelTransitionSceneScript : MonoBehaviour {

    private float _timeStarted; //Time at which this level transition was first shown
    [SerializeField] private Text _levelText; //Text displaying what level is coming up

    void Start() {
        _timeStarted = Time.time;

        //Sets the text to show the current level which is to be played
        _levelText.text = LevelManagerScript.GetLevelName()+" Start!";
    }

    void Update() {
        //If enough time has passed since showing this level transition screen...
        if(Time.time - _timeStarted > 1.5f) {
            //Transition to the actual level scene
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, LevelManagerScript.GetLevelSceneName());
        }
    }
}
