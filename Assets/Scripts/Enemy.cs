using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 10.0f;
    [SerializeField] private float visibleTime = 0.05f; //how long in total enemy will remain visible while no longer in field of vision

    private SpriteRenderer spriteRenderer;   
    private float timer; //how long left for enemy to reamin visible while no longer in field of vision
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

}
