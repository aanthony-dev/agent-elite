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
        reloadTime = 2.5f;
        damage = 5.0f;
        inaccuracy = 0.05f;
        automatic = true;
        fireRate = 0.15f;

        fov = 50.0f;
        viewDistance = 20.0f;
        shootVolume = 20.0f;
        reloadVolume = 3.0f;
    }

    void Start()
    {
        if (transform.parent.name == "Player")
        {
            ammoCount = GameObject.Find("AmmoCount").GetComponent<TMPro.TextMeshProUGUI>();
            ammoCount.text = getCurrentAmmo().ToString() + " | " + getClipSize().ToString();
        }

        firePoint = transform.parent.gameObject.transform.GetChild(0);
        bullet = Resources.Load("bullet") as GameObject;

        shootSound = Resources.Load("rifle_shot") as AudioClip;
        reloadSound = Resources.Load("rifle_reload") as AudioClip;

        firePoint.GetComponent<AudioSource>().clip = shootSound;
    }

    //shoot the gun
    public override void shoot()
    {
        if (canShoot && currentAmmo > 0 && Time.timeScale != 0)
        {
            StartCoroutine("shootRoutine");
        }
        else
        {
            //Debug.Log("Cannot Shoot");
        }
    }

    //routine to shoot the gun
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

        if (transform.parent.name == "Player") //prevent chain of enemies hearing enemies
        {
            ammoCount.text = getCurrentAmmo().ToString() + " | " + getClipSize().ToString();
            audioRadius(shootVolume); //check if any enemies heard the noise
        }
        

        for (float time = fireRate; time >= 0.0f; time -= Time.deltaTime)
        {
            yield return null;
        }
        canShoot = true;
    }

    //reload the gun
    public override void reload()
    {
        StartCoroutine("reloadRoutine");
    }

    //routine to shoot the gun
    protected IEnumerator reloadRoutine()
    {
        firePoint.GetComponent<AudioSource>().clip = reloadSound;
        firePoint.GetComponent<AudioSource>().Play();
        canShoot = false;
        reloading = true;

        if (transform.parent.name == "Player") //prevent chain of enemies hearing enemies
        {
            ammoCount.text = "Reloading";
            audioRadius(reloadVolume); //check if any enemies heard the noise
        }

        for (float time = reloadTime; time >= 0.0f; time -= Time.deltaTime)
        {
            yield return null;
        }

        currentAmmo = clipSize;
        if (transform.parent.name == "Player") //prevent chain of enemies hearing enemies
        {
            ammoCount.text = getCurrentAmmo().ToString() + " | " + getClipSize().ToString();
        }

        firePoint.GetComponent<AudioSource>().clip = shootSound;
        reloading = false;
        canShoot = true;
    }
}
