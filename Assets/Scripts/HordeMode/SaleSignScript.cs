using UnityEngine;
using UnityEngine.UI;

public class SaleSignScript : MonoBehaviour {

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
        _tooltipText.enabled = Physics2D.OverlapCircle((Vector2)_transform.position, 1f, 1 << (LayerMask.NameToLayer("Players")));
    }

    public void AttemptPurchase(PlayerController pController, PlayerInfo pInfo) {
        if (pInfo.TryToPurchase(_price)) {
            CompletePurchase(pController, pInfo);
            NewGameHUD.UpdatePlayerScoreVisual(pController.GetPlayerNumber());
        }
    }

    protected virtual void CompletePurchase(PlayerController pController, PlayerInfo player) {
        
    }

    public static string GetDefaultText() {
        return "[ E (KEYBOARD) / A (GAMEPAD) ]";
    }

}