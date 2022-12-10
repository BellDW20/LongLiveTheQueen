using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelector : MonoBehaviour {

    [SerializeField] private MenuSelection[] _selections;
    [SerializeField] private Transform _selectionIndicator;
    [SerializeField] private int[] _playersControlledBy;
    private int[] _joysticksControlledBy;

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
            _vertInput[i] = InputManager.GetVerticalInput(_joysticksControlledBy[i]);
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
            if (InputManager.GetPickupInput(_joysticksControlledBy[i])) {
                _selections[_selected].DoOnClick();
                confirmed = true;
                break;
            }
        }
        //Mouse confirm
        if(_canMouseControl && !confirmed && Input.GetMouseButtonDown(0)) {
            Collider2D selection = Physics2D.OverlapPoint(_cam.ScreenToWorldPoint(Input.mousePosition), UI_LAYER);
            if (selection && _selections[_selected].gameObject == selection.gameObject) {
                _selections[_selected].DoOnClick();
            }
        }

    }

    private void UpdateSelection() {
        _selections[_lastSelected].SetColor(_deselectedColor);
        _selections[_selected].SetColor(_selectedColor);
        _selectionIndicator.position = _selections[_selected].GetPosition() - new Vector3(0.5f * _selectionIndicator.localScale.x, 0, 0);
    }

    public void SetPlayersControlling(params int[] playersControlling) {
        print("I was called n shiet "+playersControlling[0]);
        System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace(true);
        print(t.ToString());


        _joysticksControlledBy = new int[playersControlling.Length];
        _lastVertInput = new float[playersControlling.Length];
        _vertInput = new float[playersControlling.Length];
        _canMouseControl = false;
        for (int i = 0; i < playersControlling.Length; i++) {
            _joysticksControlledBy[i] = InputManager.GetPlayerAssignedJoystick(playersControlling[i]);
            if (_joysticksControlledBy[i] == 0) { _canMouseControl = true; }
        }
    }

    public void Enable() {
        _enabled = true;
    }

    public void Disable() {
        _enabled = false;
    }

}