using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 10.0f;
    [SerializeField] private float visibleTime = 0.05f; //how long in total enemy will remain visible while no longer in field of vision
    [SerializeField] private Transform firePoint; //where the bullets spawn from
    [SerializeField] private GameObject bulletPrefab; //select bullet prefab

    private SpriteRenderer spriteRenderer;   
    private float timer; //how long left for enemy to reamin visible while no longer in field of vision
    private bool shooting;
    private bool heardPlayer;
    private float bulletForce;

    void Start()
    {
        shooting = false;
        heardPlayer = false;
        bulletForce = 30.0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("shootRoutine");
    }

    void Update()
    {
        if (spriteRenderer.enabled)
        {
            timer -= Time.deltaTime;
            //if enemy is not in field of view
            if (timer <= 0)
            {
                spriteRenderer.enabled = false; //make invisible
            }
        }
    }

    //makes this enemy visible while in player's field of view
    public void makeVisible()
    {
        spriteRenderer.enabled = true;
        timer = visibleTime;
    }

    //subtracts bullet damage from enemy's health and destroys enemy if out of health.
    public void hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Enemy Killed");
            Destroy(gameObject);
        }
    }

    private void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation) as GameObject;
        bullet.GetComponent<Bullet>().setDamage(10.0f);
        //Bullet test = bullet.GetComponent<Bullet>();
        //test.setDamage(10.0f);

        firePoint.GetComponent<AudioSource>().Play();

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }

    IEnumerator shootRoutine()
    {
        for (; ; )
        {
            shoot();
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void setHeard(bool status, Vector2 position)
    {
        heardPlayer = status;
        Debug.Log("I HEARD THE PLAYER AT" + position.ToString());
    }

}
