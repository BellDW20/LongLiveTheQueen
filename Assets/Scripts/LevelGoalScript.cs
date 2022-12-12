using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelGoalScript : MonoBehaviour {

    //The level code of the level that this goal will try to transition to when touched by a player
    [SerializeField] private Level _levelToGoTo;

    //The type of transition (full level or sublevel) made upon reaching this goal
    [SerializeField] private int _levelTransitionType;

    private bool _triggered; //Whether or not this goal has been triggered by a player
    private float _timeTriggered; //The time at which this goal was triggered by a player
    private static float _timeTakenOnGame = 0;
    private float _timeCap = 150; //Amount of time (in seconds) player must finish level in to receive bonus XP 

    private void Update() {
        //If the goal has been triggered and it has been a sufficient amount
        //of time since the triggering occurred...
        if(_triggered && Time.realtimeSinceStartup - _timeTriggered > 3) {
            //If the level to go to is GAME_CLEAR, go to game clear
            //Check if game was already won, don't want to call this 1000 times
            if(_levelToGoTo == Level.GAME_CLEAR && !LevelManagerScript.WasGameWon()) {
                LevelManagerScript.WinGame();
                SceneTransitioner.BeginTransition(SceneTransitioner.FADE_OUT, 0.5f, "GameOverScene");
            } else {
                //Go to the intended level/sublevel
                LevelManagerScript.BeginLevel(_levelToGoTo, _levelTransitionType);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        //If a player enters the goal and the goal hasn't been touched
        //by any other players yet (triggered)...
        if(other.CompareTag("Player") && !_triggered) {
            //Remember that a player has touched the goal (and the time it occurred)
            float timeTaken = LevelManagerScript.GetTimeTakenOnLevel();

            _triggered = true;
            _timeTriggered = Time.realtimeSinceStartup;
            _timeTakenOnGame += timeTaken;

            if(timeTaken < _timeCap)
            {
                foreach (PlayerInfo pInfo in LevelManagerScript.pInfos)
                {
                    if(pInfo != null)
                    {
                        pInfo.AddToScore((int)(scoreMultiplier(timeTaken) * pInfo.score - pInfo.score));
                    }
                }
            }

            //Stop all sounds, and play the level clear sound
            SoundManager.StopAllSounds();
            SoundManager.PlaySFX(SFX.LEVEL_CLEAR);

            //Pause all other game activities temporarily
            Time.timeScale = 0;
        }
    }

    private float scoreMultiplier(float timeTaken)
    {
        return (3 - (timeTaken*2) / _timeCap);
    }

    public static float GetGameTime()
    {
        return _timeTakenOnGame;
    }

    public static void SetGameTime(float time)
    {
        _timeTakenOnGame += time;
    }

}
