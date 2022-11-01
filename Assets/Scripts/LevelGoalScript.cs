using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelGoalScript : MonoBehaviour {

    //The level code of the level that this goal will try to transition to when touched by a player
    [SerializeField] private int _levelToGoTo;

    //The type of transition (full level or sublevel) made upon reaching this goal
    [SerializeField] private int _levelTransitionType;

    private bool _triggered; //Whether or not this goal has been triggered by a player
    private float _timeTriggered; //The time at which this goal was triggered by a player

    private void Update() {
        //If the goal has been triggered and it has been a sufficient amount
        //of time since the triggering occurred...
        if(_triggered && Time.realtimeSinceStartup - _timeTriggered > 3) {
            //Go to the intended level/sublevel
            LevelManagerScript.BeginLevel(_levelToGoTo, _levelTransitionType);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        //If a player enters the goal and the goal hasn't been touched
        //by any other players yet (triggered)...
        if(other.CompareTag("Player") && !_triggered) {
            //Remember that a player has touched the goal (and the time it occurred)
            _triggered = true;
            _timeTriggered = Time.realtimeSinceStartup;

            //Stop all sounds, and play the level clear sound
            SoundManager.StopAllSounds();
            SoundManager.PlaySFX(SoundManager.SFX_LEVEL_CLEAR);

            //Pause all other game activities temporarily
            Time.timeScale = 0;
        }
    }

}
