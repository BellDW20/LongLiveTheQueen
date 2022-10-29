using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlasher : MonoBehaviour {

    private Text _text;
    private float time;
    [SerializeField] private Color _color0;
    [SerializeField] private Color _color1;
    [SerializeField] private float _period;

    void Start() {
        _text = GetComponent<Text>();
    }

    void Update() {
        time += Time.deltaTime;
        float lerpFactor = 0.5f*Mathf.Sin(2*Mathf.PI*time/_period)+0.5f;
        float r = Mathf.Lerp(_color0.r, _color1.r, lerpFactor);
        float g = Mathf.Lerp(_color0.g, _color1.g, lerpFactor);
        float b = Mathf.Lerp(_color0.b, _color1.b, lerpFactor);
        _text.color = new Color(r,g,b);
    }
}
