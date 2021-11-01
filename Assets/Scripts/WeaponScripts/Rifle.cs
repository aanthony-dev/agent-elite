using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    void Awake()
    {
        canShoot = true;
        reloading = false;

        clipSize = 25;
        currentAmmo = clipSize;
        reloadTime = 3.0f;
        damage = 5.0f;
        inaccuracy = 0.05f;
        automatic = true;
        fireRate = 0.1f;

        fov = 50.0f;
        viewDistance = 20.0f;
    }

    void Start()
    {
        firePoint = transform.parent.gameObject.transform.GetChild(0);
        bullet = Resources.Load("bullet") as GameObject;
    }

    public override void shoot()
    {
        if (canShoot && currentAmmo > 0)
        {
            StartCoroutine("shootRoutine");
        }
        else
        {
            //Debug.Log("Cannot Shoot");
        }
    }

    protected IEnumerator shootRoutine()
    {
        canShoot = false;

        //add weapon innacuracy
        Vector3 v = firePoint.right;
        v[0] += Random.Range(-inaccuracy, inaccuracy);
        v[1] += Random.Range(-inaccuracy, inaccuracy);

        GameObject b = Instantiate(bullet, firePoint.position, firePoint.rotation) as GameObject;
        b.GetComponent<Bullet>().setDamage(damage);

        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        rb.AddForce(v.normalized * bulletForce, ForceMode2D.Impulse);

        firePoint.GetComponent<AudioSource>().Play();

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
}
