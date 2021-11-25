using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for weapons
public abstract class Weapon : MonoBehaviour
{
    public const float bulletForce = 30.0f;
    protected GameObject bullet;
    protected Transform firePoint;

    protected AudioClip reloadSound;
    protected AudioClip shootSound;

    protected TMPro.TextMeshProUGUI ammoCount;

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
    protected float shootVolume;
    protected float reloadVolume;

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

    //get fire rate
    public float getFireRate()
    {
        return fireRate;
    }

    //is weapon is full auto
    public bool isAutomatic()
    {
        return automatic;
    }

    //get reloading status
    public bool isReloading()
    {
        return reloading;
    }

    //check for enemies within range to hear the player making noise
    public void audioRadius(float soundRadius)
    {
        //create contact filter to only check for enemies
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Enemies");

        //check for enemies within radius
        Collider2D[] colliders = new Collider2D[20];
        int enemiesFound = Physics2D.OverlapCircle(transform.position, soundRadius, filter, colliders);

        //Debug.Log(enemiesFound.ToString() + " enemies heard the player's gun:");

        //alert each enemy of player's position
        foreach (Collider2D c in colliders)
        {
            if (!(c is null))
            {
                Debug.Log(c);
                Enemy e = c.gameObject.GetComponent<Enemy>();
                e.setHeard(true, transform.position);
            }
            else
            {
                break;
            }
        }
    }
}
