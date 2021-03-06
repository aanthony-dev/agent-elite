using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 1.0f;
    [SerializeField] private float visibleTime = 0.05f; //how long in total enemy will remain visible while no longer in field of vision
    [SerializeField] private Transform firePoint; //where the bullets spawn from
    [SerializeField] private GameObject bulletPrefab; //select bullet prefab
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem deathExplosion;

    private SpriteRenderer directionIndicator;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private float timer; //how long left for enemy to remain visible while no longer in field of vision
    private bool heardPlayer;
    private Vector2 lastHeardPosition; //where the noise came from

    private bool sawPlayer;
    private bool canSeePlayer;
    private Vector2 lastSeenPosition; //position where this enemy last saw the player

    private Weapon weapon;

    private ContactFilter2D filter;
    private float viewDistance;
    private float fov;

    void Start()
    {
        weapon = transform.GetChild(1).gameObject.GetComponent<Weapon>();
        directionIndicator = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        viewDistance = weapon.getViewDistance();
        fov = weapon.getFov();

        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Player");

        heardPlayer = false;
        sawPlayer = false;
        canSeePlayer = false;
        
        StartCoroutine("fovCheckRoutine");
    }

    void Update()
    {
        if (spriteRenderer.enabled)
        {
            timer -= Time.deltaTime;
            //if enemy is not in field of view
            if (timer <= 0)
            {
                directionIndicator.enabled = false;
                spriteRenderer.enabled = false; //make invisible
            }
        }
    }

    //makes this enemy visible while in player's field of view
    public void makeVisible()
    {
        directionIndicator.enabled = true;
        spriteRenderer.enabled = true;
        timer = visibleTime;
    }

    //subtracts bullet damage from enemy's health and destroys enemy if out of health.
    public void hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Vector3 test = transform.position;
            ParticleSystem explosion = Instantiate(deathExplosion, test, Quaternion.identity);
            Destroy(explosion, 1.0f);
            Destroy(gameObject);
        }
    }

    //set where this enemy last heard the player
    public void setHeard(bool status, Vector2 position)
    {
        heardPlayer = status;
        if (status)
        {
            lastHeardPosition = position;
        }
    }

    //set whether this enemy has heard the player
    public void setHeard(bool status)
    {
        heardPlayer = status;
    }

    //get whether this enemy heard the player or not
    public bool getHeard()
    {
        return heardPlayer;
    }

    //get the position at which this enemy last heard the player
    public Vector2 getLastHeardPosition()
    {
        return lastHeardPosition;
    }

    //set whether this enemy has seen the player
    public void setSawPlayer(bool status)
    {
        sawPlayer = status;
    }

    //get whether this enemy has seen the player before
    public bool getSawPlayer()
    {
        return sawPlayer;
    }

    //set the position where this enemy last saw the player
    public void setLastSeenPosition(Vector2 position)
    {
        lastSeenPosition = position;
    }

    //get the position at which this enemy last saw the player
    public Vector2 getLastSeenPosition()
    {
        return lastSeenPosition;
    }

    //set whether this enemy can currently see the player
    public void setCanSeePlayer(bool status, Vector2 position)
    {
        canSeePlayer = status;
    }

    //get whether this enemy can currently see the player
    public bool getCanSeePlayer()
    {
        return canSeePlayer;
    }

    //checks to see if the player is currently in this enemy's field of view.
    private void fovCheck()
    {
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
                    sawPlayer = true;
                    canSeePlayer = true;
                    setLastSeenPosition(target.position);

                    if (weapon.getCurrentAmmo() > 0)
                    {
                        StartCoroutine("enemyShootRoutine");
                    }
                    else if (weapon.getCurrentAmmo() == 0 && !weapon.isReloading())
                    {
                        StopCoroutine("enemyShootRoutine");
                        weapon.reload();
                    }
                }
                else
                {
                    StopCoroutine("enemyShootRoutine");
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

    //routine to shoot at the player
    private IEnumerator enemyShootRoutine()
    {
        float delay = weapon.getFireRate();
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            weapon.shoot();
            yield return wait;
        }
    }

}

