using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {

    public const int FADE_IN = 0; //Constant to indicate a fade in transition type
    public const int FADE_OUT = 1; //Constant to indicate a fade out transition type

    private static SceneTransitioner instance; //The scene transitioner object in the active scene
    private static float _transitionTime; //How long the current transition is
    private static float _startedTransitionTime; //When the most recent transition started
    private static string _scene; //The scene that the most recent transition is transitioning to
    private static bool _transitioning = false; //Whether or not a transition is still taking place
    private static int _type = FADE_OUT; //The type of transition being performed

    [SerializeField] private Image cover; //The black image which has its transparency changed to give fade effect

    private void Awake() {
        //makes the active scene's scene transitioner object the active
        //scene transitioner object
        instance = this;
    }

    void Start() {
        //initially, we are not transitioning
        _transitioning = false;

        //But on entering a scene, perform a fade-in transition that stays in this scene
        BeginTransition(FADE_IN, _transitionTime, null);
    }

    void Update() {
        //if we are transitioning...
        if (_transitioning) {
            //Find out how far we are into the transition (as a factor from 0 to 1)
            //Using realtimeSinceStartup allows this to work when the rest of the scene has been
            //paused with a zero time scale
            float factor = (Time.realtimeSinceStartup - _startedTransitionTime) / _transitionTime;

            //If we have finished the transition
            if (factor >= 1) {
                //indicate we have finished
                _transitioning = false;
                Time.timeScale = 1; //Reset the time scale to unpause

                //If we were fading out, load the indicated scene
                if (_type == FADE_OUT) {
                    SceneManager.LoadScene(_scene);
                }
            }

            //Set the transparency of the cover to either fade to black or from black
            cover.color = new Color(0, 0, 0, (_type==FADE_OUT?factor:(1-factor)));

            //If we're fading out, also fade out the sounds
            if(_type==FADE_OUT) {
                SoundManager.SetVolume(1-factor);
            }
        }
    }

    public static void BeginTransition(int type, float time, string scene) {
        //Don't allow spamming of transitions. Transitions are only processed
        //if there is not currently a transition occurring
        if(_transitioning) { return; }
        Time.timeScale = 0; //Pause all other action on screen
        _transitionTime = time; //Set the time to perform the transition in
        _startedTransitionTime = Time.realtimeSinceStartup; //Mark when the transition started
        _scene = scene; //Set the scene to transition to
        _type = type; //Set the type of transition (fade in / fade out)
        _transitioning = true; //Indicate that a transition is now taking place
    }

}
