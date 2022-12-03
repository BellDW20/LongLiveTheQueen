using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerHUD : MonoBehaviour {

    //Visuals
    [SerializeField] private Text _pText;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _lvlUpText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _skillPointsText;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Image _gunIcon;
    [SerializeField] private RectTransform _specialBar;

    //Internal data
    [SerializeField] private int _playerNum;
    [SerializeField] private bool _isHordeMode;
    private PlayerInfo _pInfo;

    void Start() {
        _pText.color = PlayerInfo.PLAYER_NUM_COLORS[_playerNum];
        _pInfo = LevelManagerScript.pInfos[_playerNum];
    }

    public void Refresh() {
        UpdateHealthVisual();
        UpdateScoreVisual();
        UpdateStockVisual();
    }

    public void UpdateHealthVisual() {
        int hpercent = (int)(100 * Mathf.Clamp01(_pInfo.health / _pInfo.GetMaxHealth()));
        _healthText.text = hpercent + "%";
    }

    public void UpdateScoreVisual() {
        _scoreText.text = _isHordeMode ? ("PTS: " + _pInfo.spendableScore) : ("SCORE: " + _pInfo.score);
        _lvlUpText.text = ((int)(100 * _pInfo.ProgressToNextLevel())) + "%";
        _skillPointsText.text = "LVL PTS: " + _pInfo.spendableLevels;
    }

    public void UpdateStockVisual() {
        _pText.text = "P"+(_playerNum+1)+": " + ((_pInfo.stock >= 0) ? (""+_pInfo.stock) : "DEAD");
    }

    public void UpdateSpecialVisual(Gun special) {
        _specialBar.localScale = new Vector3(1, Mathf.Clamp01(special.ReloadProgress()), 1);
    }

    public void UpdateGunVisual(Gun gun) {
        _gunIcon.sprite = gun.GetIcon();
        if (gun.IsReloading()) {
            //If the player is reloading set the text to indicate that
            _ammoText.text = "LOAD";
            _ammoText.color = Color.Lerp(Color.red, Color.white, Mathf.Clamp01(gun.ReloadProgress()));
        } else {
            //Otherwise, set the text to show the bullets left and magazine size
            _ammoText.text = gun.GetBulletsInMag() + "/" + gun.GetMagSize();
            _ammoText.color = Color.white;
        }
    }

    public void SetHordeMode(bool isHordeMode) {
        _isHordeMode = isHordeMode;
    }

}
