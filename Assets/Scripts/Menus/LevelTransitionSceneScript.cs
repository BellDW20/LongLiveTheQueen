using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTransitionSceneScript : MonoBehaviour {

    private float _timeStarted;
    [SerializeField] private Text _levelText;

    void Start() {
        _timeStarted = Time.time;
        _levelText.text = LevelManagerScript.GetLevelName()+" Start!";
    }

    void Update() {
        if(Time.time - _timeStarted > 1.5f) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, LevelManagerScript.GetLevelSceneName());
        }
    }
}
