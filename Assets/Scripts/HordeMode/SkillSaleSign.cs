using UnityEngine;

public class SkillSaleSign : SaleSignScript {

    [SerializeField] private PlayerSkill _skill;
    [SerializeField] private int _level;
    [SerializeField] private string _skillName;

    protected override void Start() {
        base.Start();
        _tooltipText.text = CommonText.GetInteractPrefix() + "\nUNLOCK " + _skillName + ":\n" + _price + " LVLS";
    }

    public override void AttemptPurchase(PlayerController pController, PlayerInfo pInfo) {
        if(HasSkill(pInfo)) {
            OnPurchaseFailure();
            return;
        }

        if (pInfo.TryToPurchaseLevel(_price)) {
            CompletePurchase(pController, pInfo);
            NewGameHUD.UpdatePlayerScoreVisual(pController.GetPlayerNumber());
            OnPurchaseSuccess();
        } else {
            OnPurchaseFailure();
        }
    }

    protected override void CompletePurchase(PlayerController pController, PlayerInfo player) {
        switch (_skill) {
            case PlayerSkill.SPREAD:
                player.spreadLevel = _level;
                break;
            case PlayerSkill.DAMAGE_SCALE:
                player.damageLevel = _level;
                break;
            case PlayerSkill.SPECIAL_COOLDOWN_SCALE:
                player.specialCooldownLevel = _level;
                break;
            case PlayerSkill.MAX_HEALTH_SCALE:
                player.maxHealthLevel = _level;
                break;
        }
    }

    private bool HasSkill(PlayerInfo pInfo) {
        switch (_skill) {
            case PlayerSkill.SPREAD:
                return pInfo.spreadLevel == _level;
            case PlayerSkill.DAMAGE_SCALE:
                return pInfo.damageLevel == _level;
            case PlayerSkill.SPECIAL_COOLDOWN_SCALE:
                return pInfo.specialCooldownLevel == _level;
            case PlayerSkill.MAX_HEALTH_SCALE:
                return pInfo.maxHealthLevel == _level;
        }
        return false;
    }

}