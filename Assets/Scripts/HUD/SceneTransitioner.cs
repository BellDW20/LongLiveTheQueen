using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {

    public const int FADE_IN = 0;
    public const int FADE_OUT = 1;

    private static SceneTransitioner instance;
    private static float _transitionTime, _startedTransitionTime;
    private static string _scene;
    private static bool _transitioning = false;
    private static int _type = FADE_OUT;

    [SerializeField] private Image cover;

    private void Awake() {
        instance = this;
    }

    void Start() {
        _transitioning = false;
        BeginTransition(FADE_IN, _transitionTime, null);
    }

    void Update() {
        if(_transitioning) {
            float factor = (Time.realtimeSinceStartup - _startedTransitionTime) / _transitionTime;
            if (factor >= 1) {
                _transitioning = false;
                Time.timeScale = 1;
                if (_type == FADE_OUT) {
                    SceneManager.LoadScene(_scene);
                }
            }
            cover.color = new Color(0, 0, 0, (_type==FADE_OUT?factor:(1-factor)));
            if(_type==FADE_OUT) {
                SoundManager.SetVolume(1-factor);
            }
        }
    }

    public static void BeginTransition(int type, float time, string scene) {
        if(_transitioning) { return; }
        Time.timeScale = 0;
        _transitionTime = time;
        _startedTransitionTime = Time.realtimeSinceStartup;
        _scene = scene;
        _type = type;
        _transitioning = true;
    }

}
