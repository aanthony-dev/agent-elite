using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    
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

        fov = 90.0f;
        viewDistance = 10.0f;
        shootVolume = 15.0f;
        reloadVolume = 3.0f;
    }

    void Start()
    {
        if (transform.parent.name == "Player")
        {
            ammoCount = GameObject.Find("AmmoCount").GetComponent<TMPro.TextMeshProUGUI>();
            ammoCount.text = getCurrentAmmo().ToString() + " / " + getClipSize().ToString();
        }

        firePoint = transform.parent.gameObject.transform.GetChild(0);
        bullet = Resources.Load("bullet") as GameObject;

        //CHANGE THIS LATER
        shootSound = firePoint.GetComponent<AudioSource>().clip;
        reloadSound = Resources.Load("revolver_reload") as AudioClip;
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
            Debug.Log("Cannot Shoot");
        }
    }

    //routine to shoot the gun
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

        //Debug.Log(getCurrentAmmo().ToString() + " / " + getClipSize().ToString());

        if (transform.parent.name == "Player") //prevent chain of enemies hearing enemies
        {
            ammoCount.text = getCurrentAmmo().ToString() + " / " + getClipSize().ToString();
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

    //routine to reload the gun
    protected IEnumerator reloadRoutine()
    {
        firePoint.GetComponent<AudioSource>().clip = reloadSound;
        firePoint.GetComponent<AudioSource>().Play();
        canShoot = false;
        reloading = true;

        if (transform.parent.name == "Player") //prevent chain of enemies hearing enemies
        {
            ammoCount.text = "RELOADING";
            audioRadius(reloadVolume); //check if any enemies heard the noise
        }

        for (float time = reloadTime; time >= 0.0f; time -= Time.deltaTime)
        {
            yield return null;
        }

        currentAmmo = clipSize;
        if (transform.parent.name == "Player") //prevent chain of enemies hearing enemies
        {
            ammoCount.text = getCurrentAmmo().ToString() + " / " + getClipSize().ToString();
        }

        firePoint.GetComponent<AudioSource>().clip = shootSound;
        reloading = false;
        canShoot = true;

        Debug.Log("Reloaded: " + getCurrentAmmo().ToString() + " / " + getClipSize().ToString());
    }
}
