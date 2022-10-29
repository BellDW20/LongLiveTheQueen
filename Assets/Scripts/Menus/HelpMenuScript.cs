using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _helpCanvas;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            _mainCanvas.SetActive(true);
            _helpCanvas.SetActive(false);
        }
    }

}
