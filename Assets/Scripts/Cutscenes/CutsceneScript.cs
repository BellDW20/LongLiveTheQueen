using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneScript : MonoBehaviour {

    [SerializeField] private Text[] _slideTexts;
    [SerializeField] private float _timeForSlide;
    [SerializeField] private float _timeForSlideDelay;
    [SerializeField] private string _sceneToLoadWhenFinished;

    private int _slideOn;
    private string _fullSlideText;
    private float _slideStartTime;

    void Start() {
        _slideOn = 0;
        _fullSlideText = _slideTexts[0].text;
        _slideTexts[0].text = "";
        _slideTexts[0].enabled = true;
        _slideStartTime = Time.time;
    }

    void Update() {
        if(_slideOn >= _slideTexts.Length) {
            return;
        }

        if(Input.anyKeyDown) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, _sceneToLoadWhenFinished);
        }

        float dt = Time.time - _slideStartTime;
        if (dt > _timeForSlide + _timeForSlideDelay) {
            advanceSlides();
            return;
        }

        int charOn = (int)Mathf.Clamp((dt / _timeForSlide)*_fullSlideText.Length, 0, _fullSlideText.Length);
        _slideTexts[_slideOn].text = _fullSlideText.Substring(0, charOn);
    }

    private void advanceSlides() {
        _slideTexts[_slideOn].enabled = false;
        _slideOn++;

        if (_slideOn >= _slideTexts.Length) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, _sceneToLoadWhenFinished);
            return;
        }

        _fullSlideText = _slideTexts[_slideOn].text;
        _slideTexts[_slideOn].text = "";
        _slideTexts[_slideOn].enabled = true;
        _slideStartTime = Time.time;
    }
}
