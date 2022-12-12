using UnityEngine;
using UnityEngine.UI;

public class GravestoneScript : MonoBehaviour {

    [SerializeField] private Text _respawnText;
    private float _timeCreated;
    private bool _showText = true;

    public void Start() {
        _timeCreated = Time.time;
    }

    public void Update() {
        if(!_showText) {
            return;
        }
        int time = Mathf.Clamp((int)(4 - Time.time + _timeCreated), 1, 3);
        _respawnText.text = "RESPAWNING IN " + time + "...";
    }

    public void DisableText() {
        _respawnText.enabled = false;
        _showText = false;
    }

}