using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun {

    [SerializeField] private float _shootDelay; //Time between successive shots
    private float _shotStartTime = float.NegativeInfinity; //Last time a shot occurred
    private bool _shooting; //Whether or not this gun is shooting

    [SerializeField] private float _reloadDelay; //Time it takes to reload
    private float _reloadStartTime = float.NegativeInfinity; //Last time reloading started
    private bool _reloading; //Whether or not this gun is reloading

    [SerializeField] private int _magSize; //The maximum number of bullets in a magazine
    private int _bulletsInMag; //The current number of bullets in the magazine

    //The maximum angle at which bullets can deviate from the aiming direction when shooting
    [SerializeField] private float _spreadAngle;

    public void Init() {
        _bulletsInMag = _magSize;
    }

    public bool CanShoot() {
        //If the time since the last shot is past the shot delay, we are no longer shooting
        _shooting = (Time.time - _shotStartTime) < _shootDelay;
        //If the time since the last reload started is past the reload delay, we are no longer reloading
        _reloading = (Time.time - _reloadStartTime) < _reloadDelay;

        //If we are either still shooting or reloading...
        if (_shooting || _reloading) {
            //We cannot shoot another round
            return false;
        } else {
            //Otherwise, the shot was successful
            _bulletsInMag--; //Remove a bullet from the magazine
            if (_bulletsInMag == 0) {
                //if we ran out of bullets, start reloading
                _bulletsInMag = _magSize;
                _reloadStartTime = Time.time;
            }
            else {
                //if we still have bullets, start waiting between shots
                _shotStartTime = Time.time;
            }
            //and let the function caller know the shot was successful
            return true;
        }
    }

    //Returns a random angle deviation within this gun's spread range (-spreadAngle to +spreadAngle)
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
        //Actively recalculate the reloading metric based on the time
        //since the last reload
        _reloading = (Time.time - _reloadStartTime) < _reloadDelay;
        return _reloading;
    }

}
