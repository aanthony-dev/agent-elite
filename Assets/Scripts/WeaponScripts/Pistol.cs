using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    void Awake()
    {
        canShoot = true;
        clipSize = 8;
        currentAmmo = clipSize;
        reloadTime = 1.0f;
        damage = 5.0f;
        fov = 60.0f;
        viewDistance = 10.0f;
        automatic = false;
        fireRate = 0.2f;
    }

    void Start()
    {
        firePoint = transform.parent.gameObject.transform.GetChild(0);
        bullet = Resources.Load("bullet") as GameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //change to GetMouseButton for automatic
        {
            shoot();
        }
    }

    public override void shoot()
    {
        if (canShoot && currentAmmo > 0)
        {
            StartCoroutine("shootRoutine");
        }
        else
        {
            Debug.Log("Cannot Shoot");
        }
    }

    protected IEnumerator shootRoutine()
    {
        canShoot = false;

        GameObject b = Instantiate(bullet, firePoint.position, firePoint.rotation) as GameObject;
        b.GetComponent<Bullet>().setDamage(damage);

        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

        currentAmmo -= 1;
        Debug.Log(getCurrentAmmo().ToString() + " / " + getClipSize().ToString());

        for (float time = fireRate; time >= 0.0f; time -= Time.deltaTime)
        {
            yield return null;
        }
        canShoot = true;
    }

    public override void reload()
    {
        StartCoroutine("reloadRoutine");
    }

    protected IEnumerator reloadRoutine()
    {
        canShoot = false;
        for (float time = reloadTime; time >= 0.0f; time -= Time.deltaTime)
        {
            yield return null;
        }
        currentAmmo = clipSize;
        canShoot = true;

        Debug.Log("Reloaded: " + getCurrentAmmo().ToString() + " / " + getClipSize().ToString());
    }

    public override int getCurrentAmmo()
    {
        return currentAmmo;
    }
    public override int getClipSize()
    {
        return clipSize;
    }

    public override float getFov()
    {
        return fov;
    }

    public override float getViewDistance()
    {
        return viewDistance;
    }
}
