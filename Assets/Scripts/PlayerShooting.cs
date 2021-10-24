using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint; //where the bullets spawn from
    [SerializeField] private GameObject bulletPrefab; //select bullet prefab

    private bool shooting;
    private float bulletForce;

    void Start()
    {
        shooting = false;
        bulletForce = 30.0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shooting = true;
            shoot();
        }
        shooting = false;
    }

    //shoot a bullet
    void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation) as GameObject;
        bullet.GetComponent<Bullet>().setDamage(10.0f);
        //Bullet test = bullet.GetComponent<Bullet>();
        //test.setDamage(10.0f);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }

    public bool isShooting()
    {
        return shooting;
    }
}
