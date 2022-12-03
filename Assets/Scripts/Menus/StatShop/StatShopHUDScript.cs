using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatShopHUDScript : MonoBehaviour {

    [SerializeField] private GameObject[] _playerShops;
    private PlayerStatShop[] _playerShopScripts;
    [SerializeField] private Transform _backdrop;

    void Start() {
        _playerShopScripts = new PlayerStatShop[4];
        for (int i = 0; i < 4; i++) {
            _playerShopScripts[i] = _playerShops[i].GetComponent<PlayerStatShop>();
        }
    }

    void Update() {
        bool exit = true;
        for(int i=0; i<LevelManagerScript.GetPlayerCount(); i++) {
            exit &= _playerShopScripts[i].IsConfirmed();
        }
        if(exit) {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SetupShop(Vector3 relativePos) {
        int players = LevelManagerScript.GetPlayerCount();
        float totalWidth = 4.5f * players;
        float startX = -totalWidth * 0.5f;
        float dx = totalWidth / players;

        for (int i = 0; i < 4; i++) {
            if (i < players) {
                _playerShops[i].SetActive(true);
                _playerShops[i].transform.position = relativePos + new Vector3(startX + (i+0.3f) * dx, 1.75f, 0);
                _playerShops[i].GetComponent<PlayerStatShop>().Reset();
            } else {
                _playerShops[i].SetActive(false);
            }
        }

        _backdrop.position = relativePos;
    }

}
