using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneScript : MonoBehaviour {

    [SerializeField] private Text[] _slideTexts; //Slides of text (in order) which the cutscene shows
    [SerializeField] private Image[] _slideImgs; //Slides of images (in order) which the cutscene shows
    [SerializeField] private float _timeForSlide; //Time that it takes to reveal the text on each slide
    [SerializeField] private float _timeForSlideDelay; //Time between when the text on a slide is fully revealed and when
                                                       //the next slide is switched to
    [SerializeField] private string _sceneToLoadWhenFinished; //Scene to load when the cutscene is finished

    private int _slideOn; //Index of the slide of text currently being displayed
    private string _fullSlideText; //The full text of the given slide
    private float _slideStartTime; //The time at which the current slide was first switched to

    void Start() {
        //begin on the first slide
        _slideOn = 0;
        //save the full text of the slide to interpolate through later
        _fullSlideText = _slideTexts[0].text;
        //showing no text
        _slideTexts[0].text = "";
        //but render this slide's content
        _slideTexts[0].enabled = true;
        _slideImgs[0].enabled = true;
        _slideStartTime = Time.time;
    }

    void Update() {
        //Don't try to show anything if we've been through all of the slides
        if(_slideOn >= _slideTexts.Length) {
            return;
        }

        //If the player presses anything, skip the cutscene
        if(Input.anyKeyDown) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, _sceneToLoadWhenFinished);
        }

        //Calculate the time we've been on this slide
        float dt = Time.time - _slideStartTime;
        //If the text has been fully revealed and we've waited the extra delay...
        if (dt > _timeForSlide + _timeForSlideDelay) {
            //move to the next slide
            advanceSlides();
            return;
        }

        //Interpolate from 0 to the length of the slide's text
        //based on how long we've been on the slide
        int charOn = (int)Mathf.Clamp((dt / _timeForSlide)*_fullSlideText.Length, 0, _fullSlideText.Length);

        //and then show that proportion of the text
        _slideTexts[_slideOn].text = _fullSlideText.Substring(0, charOn);
    }

    private void advanceSlides() {
        //make this slide invisible
        _slideTexts[_slideOn].enabled = false;
        _slideImgs[_slideOn].enabled = false;
        //move to the next slide
        _slideOn++;

        //if we've ran out of slides, the cutscene is over
        if (_slideOn >= _slideTexts.Length) {
            SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, _sceneToLoadWhenFinished);
            return;
        }

        //otherwise, save the next slide's text
        _fullSlideText = _slideTexts[_slideOn].text;
        //start by rendering no text
        _slideTexts[_slideOn].text = "";
        //allow this slide to be seen
        _slideTexts[_slideOn].enabled = true;
        _slideImgs[_slideOn].enabled = true;
        _slideStartTime = Time.time;
    }
}
