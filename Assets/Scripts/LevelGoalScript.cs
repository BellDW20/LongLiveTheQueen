using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelGoalScript : MonoBehaviour {

    [SerializeField] private int _levelToGoTo;
    [SerializeField] private int _levelTransitionType;

    private bool _triggered;
    private float _timeTriggered;

    private void Update() {
        if(_triggered && Time.realtimeSinceStartup - _timeTriggered > 3) {
            LevelManagerScript.BeginLevel(_levelToGoTo, _levelTransitionType);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !_triggered) {
            _triggered = true;
            _timeTriggered = Time.realtimeSinceStartup;
            SoundManager.StopAllSounds();
            SoundManager.PlaySFX(SoundManager.SFX_LEVEL_CLEAR);
            Time.timeScale = 0;
        }
    }

}
