using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSaleSign : SaleSignScript {

    [SerializeField] private GunType _gunTypeToSell;
    private Gun _gunToSell;

    protected override void Start() {
        GameObject gun = LevelManagerScript.GetGun(_gunTypeToSell);
        _gunToSell = gun.GetComponent<WeaponPickupScript>().GetGun();
        _tooltipText.text = CommonText.GetInteractPrefix()+"\nPURCHASE GUN / REFILL AMMO:\n" + _price + " PTS";
        _icon = (gun.GetComponent<SpriteRenderer>()).sprite;
        base.Start();
    }

    protected override void CompletePurchase(PlayerController pController, PlayerInfo pInfo) {
        pController.GiveSecondary(_gunToSell.GetCopy());
    }

}
