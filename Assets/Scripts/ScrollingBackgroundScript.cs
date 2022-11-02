using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroundScript : MonoBehaviour {

    //The backgrounds that this script scrolls
    [SerializeField] private GameObject _bkg0;
    [SerializeField] private GameObject _bkg1;

    //How to position the scrolling objects relative to the center of the screen
    [SerializeField] private int _positioning; //0=centered, 1=center bottom

    [SerializeField] private float _scrollSpeed; //The speed at which to scroll shish

    private Camera _cam; //Main camera for making scaling / positioning decisions with
    private float _wrapX; //The size of the background so we know when to wrap the background seamlessly

    void Start()
    {
        _cam = Camera.main;
        //Make the backgrounds fit within our screen
        fitToScreen(_bkg0,0);
        fitToScreen(_bkg1,1);
    }

    private void Update() {
        Vector3 pos0 = _bkg0.transform.position;
        Vector3 pos1 = _bkg1.transform.position;

        //Scroll the backgrounds at the correct speed
        pos0 = new Vector3(pos0.x - _scrollSpeed * Time.deltaTime, pos0.y, pos0.z);
        pos1 = new Vector3(pos1.x - _scrollSpeed * Time.deltaTime, pos1.y, pos1.z);

        //If either get too far to the left, wrap them back to loop seamlessly
        if (pos0.x <= -_wrapX) {
            pos0 = new Vector3(pos0.x + 2 * _wrapX, pos0.y, pos0.z);
        }
        if(pos1.x <= -_wrapX) {
            pos1 = new Vector3(pos1.x + 2 * _wrapX, pos1.y, pos1.z);
        }

        //Update the transform positions to reflect the changes
        _bkg0.transform.position = pos0;
        _bkg1.transform.position = pos1;
    }

    private void fitToScreen(GameObject bkg, int num) {
        SpriteRenderer spr = bkg.GetComponent<SpriteRenderer>();

        //Sizes of the image in pixels
        float w = spr.bounds.size.x;
        float h = spr.bounds.size.y;

        //size of the camera viewport in world space
        float screenH = _cam.orthographicSize * 2.0f;
        float screenW = screenH * _cam.aspect;

        _wrapX = screenW; //When to wrap our images in world space

        //Scales the background to fit the screen
        bkg.transform.localScale = new Vector3(screenW / w, screenW / w, 1);

        //Positions the background properly
        float posX = bkg.transform.position.x + (num == 0 ? 0 : _wrapX);
        float posY = (_positioning == 0) ? 0 : (-0.5f * screenH + 0.5f*h*(screenW/w));
        bkg.transform.position = new Vector3(posX, posY, 0);
    }
}
