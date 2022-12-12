using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatShop : MonoBehaviour {

    [SerializeField] Text _playerText;
    [SerializeField] Text _lpText;
    [SerializeField] int _playerNum;
    private PlayerInfo _pInfo;
    private bool _confirmed;

    [SerializeField] Text _healthCostText;
    [SerializeField] Text _damageCostText;
    [SerializeField] Text _cooldownCostText;
    [SerializeField] Text _spreadCostText;
    [SerializeField] TextFlasher _confirmFlash;

    [SerializeField] MenuSelector _statsMenu;

    void Start() {
        _playerText.color = PlayerInfo.PLAYER_NUM_COLORS[_playerNum];
        _playerText.text = "PLAYER " + (_playerNum + 1);
        _pInfo = LevelManagerScript.pInfos[_playerNum];
    }

    void Update() {
        if(_confirmed) { return; }

        _lpText.text = "LVL PTS: " + _pInfo.spendableLevels;

        if (_pInfo.maxHealthLevel != PlayerInfo.MAX_HEALTH_LEVELS) {
            _healthCostText.text = PlayerInfo.MAX_HEALTH_COST[_pInfo.maxHealthLevel + 1] + " LP";
        } else {
            _healthCostText.text = "MAX!";
        }

        if (_pInfo.damageLevel != PlayerInfo.DAMAGE_LEVELS) {
            _damageCostText.text = PlayerInfo.DAMAGE_COST[_pInfo.damageLevel + 1] + " LP";
        }
        else {

            _damageCostText.text = "MAX!";
        }

        if (_pInfo.specialCooldownLevel != PlayerInfo.SPECIAL_COOLDOWN_LEVELS) {
            _cooldownCostText.text = PlayerInfo.SPECIAL_COOLDOWN_COST[_pInfo.specialCooldownLevel + 1] + " LP";
        }
        else {
            _cooldownCostText.text = "MAX!";
        }

        if (_pInfo.spreadLevel != PlayerInfo.SPREAD_LEVELS) {
            _spreadCostText.text = PlayerInfo.SPREAD_COST[_pInfo.spreadLevel + 1] + " LP";
        }
        else {
            _spreadCostText.text = "MAX!";
        }
    }
    public void Reset() {
        _confirmed = false;
        _confirmFlash.SetPeriod(0);
        _confirmFlash.ResetValues();
        _statsMenu.Enable();
    }

    private void UpgradeSkill(PlayerSkill skill) {
        if (_pInfo.TryToPurchaseSkill(skill)) {
            SoundManager.PlaySFX(SFX.MENU_CONFIRM);
            NewGameHUD.UpdatePlayerScoreVisual(_playerNum);
        }
        else {
            SoundManager.PlaySFX(SFX.MENU_ERROR);
        }
    }

    public void UpgradeHealth() {
        UpgradeSkill(PlayerSkill.MAX_HEALTH_SCALE);
        NewGameHUD.UpdatePlayerHealthVisual(_playerNum);
    }

    public void UpgradeDamage() {
        UpgradeSkill(PlayerSkill.DAMAGE_SCALE);
    }

    public void UpgradeCooldown() {
        UpgradeSkill(PlayerSkill.SPECIAL_COOLDOWN_SCALE);
    }

    public void UpgradeSpread() {
        UpgradeSkill(PlayerSkill.SPREAD);
    }

    public void Confirm() {
        _confirmed = true;
        _confirmFlash.SetPeriod(0.5f);
        _statsMenu.Disable();
    }
    public bool IsConfirmed() {
        return _confirmed;
    }

}
