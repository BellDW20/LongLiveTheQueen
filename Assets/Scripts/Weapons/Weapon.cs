using UnityEngine;

[System.Serializable]
public class Weapon {

    [SerializeField] private float _cooldownTime; //Time between successive uses
    private float _lastUseTime = float.NegativeInfinity; //Last time weapon was used
    private bool _coolingDown; //Whether or not this weapon is still in its cooldown phase

    public virtual bool CanUse() {
        _coolingDown = (Time.time - _lastUseTime) < _cooldownTime;
        return !_coolingDown;
    }

    public virtual void Use() {
        _lastUseTime = Time.time;
    }

}