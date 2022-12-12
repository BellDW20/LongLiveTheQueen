using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class WeaponPickupScript : MonoBehaviour {

    public const float ACTIVATION_RANGE = 0.35f;

    private int PLAYER_MASK;

    [SerializeField] private Gun _pickup;
    [SerializeField] private Text _tooltipText;
    private Transform _transform;

    public void Start() {
        PLAYER_MASK = 1 << (LayerMask.NameToLayer("Players"));
        _transform = transform;
        _tooltipText.text = CommonText.GetInteractPrefix() + "\n PICK UP GUN";
    }

    public void Update() {
        _tooltipText.enabled = Physics2D.OverlapCircle((Vector2)_transform.position, ACTIVATION_RANGE, PLAYER_MASK);
    }

    public Gun GetGun()
    {
        return _pickup;
    }

    public void SetGun(Gun g)
    {
        _pickup = g;
    }
}
