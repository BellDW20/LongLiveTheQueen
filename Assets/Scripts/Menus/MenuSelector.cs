using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelector : MonoBehaviour {

    [SerializeField] private MenuSelection[] _selections;
    [SerializeField] private Transform _selectionIndicator;
    [SerializeField] private int[] _playersControlledBy;

    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _deselectedColor;

    private int _lastSelected, _selected;
    private float[] _lastVertInput, _vertInput;
    private bool _init;
    private bool _canMouseControl;

    private Camera _cam;
    private int UI_LAYER;

    private bool _enabled;

    void Start() {
        _lastSelected = 0;
        _selected = 0;
        _init = false;
        _enabled = true;

        SetPlayersControlling(_playersControlledBy);

        _cam = Camera.main;
        UI_LAYER = 1 << LayerMask.NameToLayer("UI");

        for(int i=0; i<_selections.Length; i++) {
            _selections[i].SetIndex(i);
        }
    }

    void Update() {
        if(!_enabled) { return; }

        if(!_init) {
            _init = true;
            UpdateSelection();
        }

        _lastSelected = _selected;
        //Gamepad selection
        for (int i = 0; i < _playersControlledBy.Length; i++) {
            _lastVertInput[i] = _vertInput[i];
            _vertInput[i] = InputManager.GetVerticalInput(_playersControlledBy[i]);
            if (_lastVertInput[i] == 0 && _vertInput[i] != 0) {
                if(_vertInput[i] < 0) { _selected++; }
                else { _selected--; }
            }
        }
        //MouseSelection
        if (_canMouseControl) {
            for (int i = 0; i < _selections.Length; i++) {
                Collider2D selection = Physics2D.OverlapPoint(_cam.ScreenToWorldPoint(Input.mousePosition), UI_LAYER);
                if (selection) {
                    _selected = selection.GetComponent<MenuSelection>().GetIndex();
                }
            }
        }
        _selected = Mathf.Clamp(_selected, 0, _selections.Length - 1);

        if (_selected != _lastSelected) {
            SoundManager.PlaySFX(SFX.MENU_SELECT);
            UpdateSelection();
        }

        //Gamepad confirm
        bool confirmed = false;
        for(int i=0; i<_playersControlledBy.Length; i++) {
            if (InputManager.GetPickupInput(_playersControlledBy[i])) {
                _selections[_selected].DoOnClick();
                confirmed = true;
                break;
            }
        }
        //Mouse confirm
        if(_canMouseControl && !confirmed && Input.GetMouseButtonDown(0)) {
            _selections[_selected].DoOnClick();
        }

    }

    private void UpdateSelection() {
        _selections[_lastSelected].SetColor(_deselectedColor);
        _selections[_selected].SetColor(_selectedColor);
        _selectionIndicator.position = _selections[_selected].GetPosition() - new Vector3(0.5f * _selectionIndicator.localScale.x, 0, 0);
    }

    public void SetPlayersControlling(params int[] playersControlling) {
        _playersControlledBy = playersControlling;
        _lastVertInput = new float[_playersControlledBy.Length];
        _vertInput = new float[_playersControlledBy.Length];
        _canMouseControl = false;
        for (int i = 0; i < _playersControlledBy.Length; i++) {
            _playersControlledBy[i] = InputManager.GetPlayerAssignedJoystick(_playersControlledBy[i]);
            if (_playersControlledBy[i] == 0) { _canMouseControl = true; }
        }
    }

    public void Enable() {
        _enabled = true;
    }

    public void Disable() {
        _enabled = false;
    }

}