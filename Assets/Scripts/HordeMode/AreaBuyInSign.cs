using UnityEngine;

public class AreaBuyInSign : SaleSignScript {

    [SerializeField] private GameObject _barrier;
    [SerializeField] private HordeModeArea _areaToUnlock;
    [SerializeField] private string _areaName;

    protected override void Start() {
        base.Start();
        _tooltipText.text = CommonText.GetInteractPrefix() + "\nACCESS TO " + _areaName + ":\n" + _price + " PTS";
    }

    protected override void CompletePurchase(PlayerController pController, PlayerInfo player) {
        HordeModeMGR.UnlockArea(_areaToUnlock);
        Destroy(_barrier);
        Destroy(gameObject);
    }

}