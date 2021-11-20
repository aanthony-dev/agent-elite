using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 10.0f;
    [SerializeField] private float visibleTime = 0.05f; //how long in total enemy will remain visible while no longer in field of vision
    [SerializeField] private Transform firePoint; //where the bullets spawn from
    [SerializeField] private GameObject bulletPrefab; //select bullet prefab
    [SerializeField] private LayerMask layerMask;

    private SpriteRenderer spriteRenderer;
    private float timer; //how long left for enemy to reamin visible while no longer in field of vision
    private bool heardPlayer;
    private Vector2 noiseLocation; //where the noise came from
    private bool canSeePlayer;
    private float bulletForce;

    private ContactFilter2D filter;
    private float viewDistance = 10.0f; //CHANGE LATER DEPENDING ON WEAPON
    private float fov = 60.0f; //CHANGE LATER DEPENDING ON WEAPON 

    void Start()
    {
        heardPlayer = false;
        canSeePlayer = false;
        bulletForce = 30.0f;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //StartCoroutine("shootRoutine");

        StartCoroutine("fovCheckRoutine");

        filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Player");
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

    //TEMPORARY TEST SHOOTING METHOD
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

    //TEMPORARY TEST SHOOTING ROUTINE
    IEnumerator shootRoutine()
    {
        for (; ; )
        {
            shoot();
            yield return new WaitForSeconds(1.0f);
        }
    }

    //set where this enemy last heard the player
    public void setHeard(bool status, Vector2 position)
    {
        heardPlayer = status;
        noiseLocation = position;
        Debug.Log("I HEARD THE PLAYER AT" + noiseLocation.ToString());
    }

    //get whether this enemy heard the player or not
    public bool getHeard()
    {
        return heardPlayer;
    }

    //get the position at which this enemy heard the player
    public Vector2 getNoiseLocation()
    {
        return noiseLocation;
    }

    //set whether this enemy can currently see the player
    public void setCanSeePlayer(bool status, Vector2 position)
    {
        canSeePlayer = status;
        Debug.Log("I SEE THE PLAYER AT" + position.ToString());
    }

    //get whether this enemy can currently see the player
    public bool getCanSeePlayer()
    {
        return canSeePlayer;
    }

    //checks to see if the player is currently in this enemy's field of view.
    private void fovCheck()
    {
        //create contact filter to only check for enemies
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Player");

        Collider2D[] results = new Collider2D[1];
        int enemiesFound = Physics2D.OverlapCircle(transform.position, viewDistance, filter, results);

        if (!(results[0] is null))
        {
            Transform target = results[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(transform.right, directionToTarget) < fov)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, layerMask))
                {
                    canSeePlayer = true;
                    Debug.Log("I CAN SEE THE PLAYER");
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    //routine to run the fov check periodically
    private IEnumerator fovCheckRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            fovCheck();
        }
    }
}

