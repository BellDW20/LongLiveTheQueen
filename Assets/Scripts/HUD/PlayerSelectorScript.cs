using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectorScript : MonoBehaviour {

    [SerializeField] private Text _playerText;
    [SerializeField] private Text _selectionText;
    public int _joystickNumber;
    public int _selection;

    private bool _movedLast;
    void Update() {
        float xInput = InputManager.GetHorizontalInput(_joystickNumber);
        if (xInput < -0.5f && !_movedLast) {
            _selection--;
            UpdateSelection();
        } else if (xInput > 0.5f && !_movedLast) {
            _selection++;
            UpdateSelection();
        }
        _movedLast = Mathf.Abs(xInput)>0.5f;
    }

    private void UpdateSelection() {
        if(_selection < 0) { _selection = 3; }
        else if (_selection >= 4) { _selection = 0; }
        _selectionText.text = PlayerInfo.PLAYER_NAME[_selection];
    }

    public void Init(int playerNumber) {
        _joystickNumber = InputManager.GetPlayerAssignedJoystick(playerNumber);
        _playerText.text = "Player "+(playerNumber+1);
        _playerText.color = PlayerInfo.PLAYER_NUM_COLORS[playerNumber];
        UpdateSelection();
    }

    public PlayerType GetSelection() {
        return (PlayerType)_selection;
    }

}
