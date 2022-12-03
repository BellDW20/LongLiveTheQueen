using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelector : MonoBehaviour {

    [SerializeField] private MenuSelection[] _selections;
    [SerializeField] private Transform _selectionIndicator;
    private int _lastSelected, _selected;
    private float[] _lastVertInput, _vertInput;
    private bool _init;

    private Camera _cam;
    private int UI_LAYER;

    void Start() {
        _lastSelected = 0;
        _selected = 0;
        _init = false;

        _lastVertInput = new float[4];
        _vertInput = new float[4];

        _cam = Camera.main;
        UI_LAYER = 1 << LayerMask.NameToLayer("UI");

        for(int i=0; i<_selections.Length; i++) {
            _selections[i].SetIndex(i);
        }
    }

    void Update() {
        if(!_init) {
            _init = true;
            UpdateSelection();
        }

        _lastSelected = _selected;
        //Gamepad selection
        for (int i = 0; i < 4; i++) {
            _lastVertInput[i] = _vertInput[i];
            _vertInput[i] = InputManager.GetVerticalInput(i);
            if (_lastVertInput[i] == 0 && _vertInput[i] != 0) {
                if(_vertInput[i] < 0) { _selected++; }
                else { _selected--; }
            }
        }
        //MouseSelection
        for(int i=0; i<_selections.Length; i++) {
            Collider2D selection = Physics2D.OverlapPoint(_cam.ScreenToWorldPoint(Input.mousePosition), UI_LAYER);
            if(selection) {
                _selected = selection.GetComponent<MenuSelection>().GetIndex();
            }
        }
        _selected = Mathf.Clamp(_selected, 0, _selections.Length - 1);

        if (_selected != _lastSelected) {
            UpdateSelection();
        }

        //Gamepad confirm
        bool confirmed = false;
        for(int i=0; i<4; i++) {
            if (InputManager.GetPickupInput(i)) {
                _selections[_selected].DoOnClick();
                confirmed = true;
                break;
            }
        }
        //Mouse confirm
        if(!confirmed && Input.GetMouseButtonDown(0)) {
            _selections[_selected].DoOnClick();
        }

    }

    private void UpdateSelection() {
        _selections[_lastSelected].Deselect();
        _selections[_selected].Select();
        _selectionIndicator.position = _selections[_selected].GetPosition() - new Vector3(0.5f * _selectionIndicator.localScale.x, 0, 0);
    }

}