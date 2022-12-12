using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectorScript : MonoBehaviour {

    [SerializeField] private Text _playerText;
    [SerializeField] private Text _selectionText;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Text _characterWeaponText;
    [SerializeField] private Text _characterSpecialText;

    [SerializeField] private Sprite[] CHARACTER_IMAGES;
    private static string[] CHARACTER_WEAPON_NAMES = new string[] { "Assault Rifle", "Sniper Rifle", "Flamethrower" };
    private static string[] CHARACTER_SPECIAL_NAMES = new string[] { "Molotov Cocktail", "Piercing Shot", "Wall of Flame" };

    public int _joystickNumber;
    public int _selection;

    private bool _movedLast;

    private void Start() {
        UpdateSelection();
    }

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
        if(_selection < 0) { _selection = 2; }
        else if (_selection >= 3) { _selection = 0; }
        _selectionText.text = PlayerInfo.PLAYER_NAME[_selection];
        _characterImage.sprite = CHARACTER_IMAGES[_selection];
        _characterWeaponText.text = "WEAPON: "+CHARACTER_WEAPON_NAMES[_selection];
        _characterSpecialText.text = "SPECIAL: "+CHARACTER_SPECIAL_NAMES[_selection];
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
