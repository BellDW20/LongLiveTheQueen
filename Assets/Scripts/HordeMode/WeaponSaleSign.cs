using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSaleSign : MonoBehaviour {

    [SerializeField] private Text _tooltipText;
    [SerializeField] private SpriteRenderer _gunIcon;

    [SerializeField] private GunType _gunTypeToSell;
    [SerializeField] private int _gunPrice;

    private Gun _gunToSell;
    private Transform _transform;

    void Start() {
        _transform = transform;

        GameObject gun = LevelManagerScript.GetGun(_gunTypeToSell);
        _gunToSell = gun.GetComponent<WeaponPickupScript>().GetGun();

        _tooltipText.text = "[ E (KEYBOARD) / A (GAMEPAD) ]\nPURCHASE GUN / REFILL AMMO:\n" + _gunPrice + " PTS";
        _gunIcon.sprite = (gun.GetComponent<SpriteRenderer>()).sprite;
    }

    void Update() {
        _tooltipText.enabled = Physics2D.OverlapCircle((Vector2)_transform.position, 1f, 1<<(LayerMask.NameToLayer("Players")));
    }

    public int GetPrice() {
        return _gunPrice;
    }

    public Gun GetGun() {
        return _gunToSell.GetCopy();
    }

}
