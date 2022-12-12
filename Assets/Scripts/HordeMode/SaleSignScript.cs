using UnityEngine;
using UnityEngine.UI;

public class SaleSignScript : MonoBehaviour {

    public const float ACTIVATION_RANGE = 1f;

    [SerializeField] protected Text _tooltipText;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected int _price;

    [SerializeField] protected SpriteRenderer _iconRenderer;
    protected Transform _transform;

    protected virtual void Start() {
        _transform = transform;
        _iconRenderer.sprite = _icon;
    }

    protected virtual void Update() {
        _tooltipText.enabled = Physics2D.OverlapCircle((Vector2)_transform.position, ACTIVATION_RANGE, 1 << (LayerMask.NameToLayer("Players")));
    }

    public virtual void AttemptPurchase(PlayerController pController, PlayerInfo pInfo) {
        if (pInfo.TryToPurchase(_price)) {
            CompletePurchase(pController, pInfo);
            NewGameHUD.UpdatePlayerScoreVisual(pController.GetPlayerNumber());
            OnPurchaseSuccess();
        } else {
            OnPurchaseFailure();
        }
    }

    protected virtual void CompletePurchase(PlayerController pController, PlayerInfo player) {
        
    }

    protected void OnPurchaseSuccess() {
        SoundManager.PlaySFX(SFX.MENU_CONFIRM);
    }

    protected void OnPurchaseFailure() {
        SoundManager.PlaySFX(SFX.MENU_ERROR);
    }

}