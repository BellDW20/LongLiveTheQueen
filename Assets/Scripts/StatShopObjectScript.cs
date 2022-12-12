using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatShopObjectScript : MonoBehaviour {

    public const float ACTIVATION_RANGE = 0.95f;

    [SerializeField] private GameObject _statShopHUD;
    [SerializeField] private Text _tooltipText;
    private int PLAYER_MASK;
    private Transform _transform;
    private Camera _cam;

    void Start() {
        PLAYER_MASK = 1 << (LayerMask.NameToLayer("Players"));
        _transform = transform;
        _cam = Camera.main;
        _tooltipText.text = CommonText.GetInteractPrefix() + "\n OPEN SHOP";
    }

    private void Update() {
        _tooltipText.enabled = (Time.deltaTime != 0) && Physics2D.OverlapCircle((Vector2)_transform.position, ACTIVATION_RANGE, PLAYER_MASK);
    }

    public void OpenShop() {
        _statShopHUD.SetActive(true);
        _statShopHUD.GetComponent<StatShopHUDScript>().SetupShop(_cam.transform.position);
        Time.timeScale = 0;
    }

}
