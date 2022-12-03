using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuSelection : MonoBehaviour {

    [SerializeField] Text _text;
    [SerializeField] UnityEvent _onClick;
    private int _index;

    private BoxCollider2D _collider;

    public void Start() {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void SetColor(Color color) {
        _text.color = color;
    }

    public void DoOnClick() {
        _onClick.Invoke();
    }
    public Vector3 GetPosition() {
        return transform.position - 0.75f * new Vector3(_collider.bounds.size.x, 0, 0);
    }
    public void SetIndex(int index) {
        _index = index;
    }

    public int GetIndex() {
        return _index;
    }

}
