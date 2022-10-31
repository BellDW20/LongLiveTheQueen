using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _mainCanvas; //Canvas full of main menu UI objects
    [SerializeField] private GameObject _helpCanvas; //Canvas full of help menu UI objects

    void Update() {
        //Since we're in the help menu, if we press escape, hide
        //the help menu and show the main menu
        if(Input.GetKeyDown(KeyCode.Escape)) {
            _mainCanvas.SetActive(true);
            _helpCanvas.SetActive(false);
        }
    }

}
