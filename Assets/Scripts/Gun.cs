using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun {

    [SerializeField] private float _shootDelay;
    private float _shotStartTime = float.NegativeInfinity;
    private bool _shooting;

    [SerializeField] private float _reloadDelay;
    private float _reloadStartTime = float.NegativeInfinity;
    private bool _reloading;

    [SerializeField] private int _magSize;
    private int _bulletsInMag;

    [SerializeField] private float _spreadAngle;

    public void Init() {
        _bulletsInMag = _magSize;
    }

    public bool CanShoot() {
        _shooting = (Time.time - _shotStartTime) < _shootDelay;
        _reloading = (Time.time - _reloadStartTime) < _reloadDelay;

        if (_shooting || _reloading) {
            return false;
        } else {
            _bulletsInMag--;
            if (_bulletsInMag == 0) {
                _bulletsInMag = _magSize;
                _reloadStartTime = Time.time;
            }
            else {
                _shotStartTime = Time.time;
            }
            return true;
        }
    }

    public float GetSpreadAngle() {
        return Mathf.PI*Random.Range(-_spreadAngle, _spreadAngle)/180.0f;
    }

    public int GetBulletsInMag() {
        return _bulletsInMag;
    }

    public int GetMagSize() {
        return _magSize;
    }

    public bool IsReloading() {
        _reloading = (Time.time - _reloadStartTime) < _reloadDelay;
        return _reloading;
    }

}
