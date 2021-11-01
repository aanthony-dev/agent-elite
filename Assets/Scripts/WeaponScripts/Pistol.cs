using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    AudioClip reloadSound;
    AudioClip shootSound;
    void Awake()
    {
        canShoot = true;
        reloading = false;

        clipSize = 8;
        currentAmmo = clipSize;
        reloadTime = 1.0f;
        damage = 5.0f;
        inaccuracy = 0.05f;
        automatic = false;
        fireRate = 0.2f;

        fov = 60.0f;
        viewDistance = 10.0f;
    }

    void Start()
    {
        firePoint = transform.parent.gameObject.transform.GetChild(0);
        bullet = Resources.Load("bullet") as GameObject;

        shootSound = firePoint.GetComponent<AudioSource>().clip;
        reloadSound = Resources.Load("revolver_reload") as AudioClip;
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

        //add weapon inaccuracy
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

        firePoint.GetComponent<AudioSource>().clip = reloadSound;
        firePoint.GetComponent<AudioSource>().Play();
        canShoot = false;
        reloading = true;
        for (float time = reloadTime; time >= 0.0f; time -= Time.deltaTime)
        {
            yield return null;
        }
        currentAmmo = clipSize;

        firePoint.GetComponent<AudioSource>().clip = shootSound;
        reloading = false;
        canShoot = true;

        Debug.Log("Reloaded: " + getCurrentAmmo().ToString() + " / " + getClipSize().ToString());
    }
}
