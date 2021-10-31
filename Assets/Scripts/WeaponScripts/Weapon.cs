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

    protected int clipSize;
    protected int currentAmmo;
    protected float reloadTime;
    protected float damage;
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
    public abstract int getCurrentAmmo();

    //get maximum ammo amount
    public abstract int getClipSize();

    //get field of view degree range
    public abstract float getFov();

    //get view distance
    public abstract float getViewDistance();

}
