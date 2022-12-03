using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuSelection : MonoBehaviour {

    [SerializeField] Text _text;
    [SerializeField] UnityEvent _onClick;
    private int _index;

    private Vector3 _position;

    public void Start() {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        _position = transform.position - 0.75f * new Vector3(collider.bounds.size.x, 0, 0);
    }

    public void Select() {
        _text.color = 0.75f*Color.black;
    }

    public void Deselect() {
        _text.color = Color.black;
    }

    public void DoOnClick() {
        _onClick.Invoke();
    }
    public Vector3 GetPosition() {
        return _position;
    }
    public void SetIndex(int index) {
        _index = index;
    }

    public int GetIndex() {
        return _index;
    }

}
