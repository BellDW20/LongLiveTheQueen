using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun : Weapon {

    [SerializeField] protected float _reloadDelay; //Time it takes to reload
    private float _reloadStartTime = float.NegativeInfinity; //Last time reloading started
    private bool _reloading; //Whether or not this gun is reloading
    private bool _wasInited = false;

    [SerializeField] protected int _magSize; //The maximum number of bullets in a magazine
    private int _bulletsInMag; //The current number of bullets in the magazine

    //The maximum angle at which bullets can deviate from the aiming direction when shooting
    [SerializeField] protected float _spreadAngle;
    [SerializeField] protected float _shotVelocity; // The speed at which projectiles exit the gun

    [SerializeField] protected GameObject _projectile; //The projectile this gun shoots

    [SerializeField] protected Sprite _icon;
    [SerializeField] protected SFX _shotSound;

    [SerializeField] private GunType _type;

    public virtual void Init() {
        if (_wasInited)
        {
            return;
        }
        _bulletsInMag = _magSize;
        _wasInited = true;
    }

    public void Reload()
    {
        if (!IsReloading())
        {
            _bulletsInMag = _magSize;
            _reloadStartTime = Time.time;
        }
    }

    public override bool CanUse() {
        //If the time since the last reload started is past the reload delay, we are no longer reloading
        _reloading = (Time.time - _reloadStartTime) < _reloadDelay;

        //If we are either still shooting or reloading...
        if (IsShooting() || _reloading) {
            //We cannot shoot another round
            return false;
        } else {
            //and let the function caller know the shot was successful
            return true;
        }
    }
    public void Shoot(Vector3 position, int playerNumber, Vector2 direction) {
        if(!CanUse()) { return; }

        //Otherwise, the shot was successful
        _bulletsInMag--; //Remove a bullet from the magazine
        if (_bulletsInMag == 0) {
            //if we ran out of bullets, start reloading
            _bulletsInMag = _magSize;
            _reloadStartTime = Time.time;
        }
        base.Use();

        //Play shot sound effect and create bullet at player's location
        SoundManager.PlaySFX(_shotSound);
        GameObject tempBullet = Object.Instantiate(_projectile, position, Quaternion.identity);

        //Determine shot angle from where the player is looking 
        float shotAngle = Mathf.Atan2(
            direction.y,
            direction.x
        ) + GetSpreadAngle();

        //Determine shot direction
        Vector2 shotDirection = new Vector2(Mathf.Cos(shotAngle), Mathf.Sin(shotAngle));

        //Make sure that the bullet is facing in the direction it gets shot
        tempBullet.transform.up = shotDirection;
        //Set velocity and the player who created the shot
        tempBullet.GetComponent<Rigidbody2D>().velocity = _shotVelocity * shotDirection;
        tempBullet.GetComponent<PlayerProjectileScript>().SetPlayerCreatedBy(playerNumber);
    }

    //Returns a random angle deviation within this gun's spread range (-spreadAngle to +spreadAngle)
    public float GetSpreadAngle() {
        return Mathf.PI*Random.Range(-_spreadAngle, _spreadAngle)/180.0f;
    }

    public float GetShotVelocity() {
        return _shotVelocity;
    }

    public int GetBulletsInMag() {
        return _bulletsInMag;
    }

    public int GetMagSize() {
        return _magSize;
    }

    public bool IsShooting() {
        return !base.CanUse();
    }

    public Sprite GetIcon() {
        return _icon;
    }

    public void SetProjectile(GameObject newProjectile)
    {
        _projectile = newProjectile;
    }

    public bool IsReloading() {
        //Actively recalculate the reloading metric based on the time
        //since the last reload
        _reloading = (Time.time - _reloadStartTime) < _reloadDelay;
        return _reloading;
    }

    public GameObject GetProjectile() {
        return _projectile;
    }

    public GunType GetGunType() {
        return _type;
    }

    public Gun GetCopy() {
        Gun copy = new Gun();
        this.CopyInto(copy);
        copy._reloadDelay = _reloadDelay;
        copy._reloadStartTime = _reloadStartTime;
        copy._reloading = _reloading;
        copy._wasInited = _wasInited;
        copy._magSize = _magSize;
        copy._bulletsInMag = _bulletsInMag;
        copy._spreadAngle = _spreadAngle;
        copy._shotVelocity = _shotVelocity;
        copy._projectile = _projectile;
        copy._icon = _icon;
        copy._shotSound = _shotSound;
        copy._type = _type;
        return copy;
    }

}
