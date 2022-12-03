using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatShopObjectScript : MonoBehaviour {

    [SerializeField] private GameObject _statShopHUD;
    private Camera _cam;
    void Start() {
        _cam = Camera.main;
    }

    void OnCollisionEnter2D(Collision2D c) {
        if(c.gameObject.CompareTag("Player")) {
            _statShopHUD.SetActive(true);
            _statShopHUD.GetComponent<StatShopHUDScript>().SetupShop(_cam.transform.position);
            Time.timeScale = 0;
        }
    }

}
