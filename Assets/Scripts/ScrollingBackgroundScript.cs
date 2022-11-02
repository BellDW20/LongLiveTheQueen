using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroundScript : MonoBehaviour {

    [SerializeField] private GameObject _bkg0;
    [SerializeField] private GameObject _bkg1;
    [SerializeField] private int _positioning; //0=centered, 1=center bottom
    [SerializeField] private float _scrollSpeed;

    private Camera _cam;
    private float _wrapX;

    void Start()
    {
        _cam = Camera.main;
        fitToScreen(_bkg0,0);
        fitToScreen(_bkg1,1);
    }

    private void Update() {
        Vector3 pos0 = _bkg0.transform.position;
        Vector3 pos1 = _bkg1.transform.position;
        pos0 = new Vector3(pos0.x - _scrollSpeed * Time.deltaTime, pos0.y, pos0.z);
        pos1 = new Vector3(pos1.x - _scrollSpeed * Time.deltaTime, pos1.y, pos1.z);
        if (pos0.x <= -_wrapX) {
            pos0 = new Vector3(pos0.x + 2 * _wrapX, pos0.y, pos0.z);
        }
        if(pos1.x <= -_wrapX) {
            pos1 = new Vector3(pos1.x + 2 * _wrapX, pos1.y, pos1.z);
        }
        _bkg0.transform.position = pos0;
        _bkg1.transform.position = pos1;
    }

    private void fitToScreen(GameObject bkg, int num) {
        SpriteRenderer spr = bkg.GetComponent<SpriteRenderer>();
        float w = spr.bounds.size.x;
        float h = spr.bounds.size.y;

        float screenH = _cam.orthographicSize * 2.0f;
        float screenW = screenH * _cam.aspect;

        _wrapX = screenW;

        bkg.transform.localScale = new Vector3(screenW / w, screenW / w, 1);

        float posX = bkg.transform.position.x + (num == 0 ? 0 : _wrapX);
        float posY = (_positioning == 0) ? 0 : (-0.5f * screenH + 0.5f*h*(screenW/w));

        bkg.transform.position = new Vector3(posX, posY, 0);
    }
}
