using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlasher : MonoBehaviour {

    private Text _text; //Text to flash
    private float _time; //Time this text has been in the scene
    [SerializeField] private Color _color0;
    [SerializeField] private Color _color1;
    [SerializeField] private float _period;

    void Start() {
        _text = GetComponent<Text>();
    }

    void Update() {
        _time += Time.deltaTime; //Update how long this text has existed

        //Calculate a factor between 0 and 1 depending on how long the
        //text has existed, following a sinusoidal pattern with the given period
        float lerpFactor = 0.5f*Mathf.Sin(2*Mathf.PI* _time / _period)+0.5f;

        //Calculate the intermediate color depending on this interpolation factor
        float r = Mathf.Lerp(_color0.r, _color1.r, lerpFactor);
        float g = Mathf.Lerp(_color0.g, _color1.g, lerpFactor);
        float b = Mathf.Lerp(_color0.b, _color1.b, lerpFactor);

        //Make the text have this color
        _text.color = new Color(r,g,b);
    }
}
