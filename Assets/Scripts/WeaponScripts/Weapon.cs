using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for weapons
public abstract class Weapon : MonoBehaviour
{
    public const float bulletForce = 30.0f;
    protected GameObject bullet;
    protected Transform firePoint;

    protected bool canShoot;
    protected bool reloading;

    protected int clipSize;
    protected int currentAmmo;
    protected float reloadTime;
    protected float damage;
    protected float inaccuracy;
    protected bool automatic;
    protected float fireRate;

    protected float fov;
    protected float viewDistance;

    //public Attachment[] attachments; ADD ATTACHMENTS LATER

    //shoot a bullet
    public abstract void shoot();

    //reload weapon
    public abstract void reload();

    //get current ammo amount
    public int getCurrentAmmo()
    {
        return currentAmmo;
    }

    //get maximum ammo amount
    public int getClipSize()
    {
        return clipSize;
    }

    //get field of view degree range
    public float getFov()
    {
        return fov;
    }

    //get view distance
    public float getViewDistance()
    {
        return viewDistance;
    }

    //is weapon is full auto
    public bool isAutomatic()
    {
        return automatic;
    }

    public bool isReloading()
    {
        return reloading;
    }
}
