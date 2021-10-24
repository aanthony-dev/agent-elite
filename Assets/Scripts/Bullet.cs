using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeSpan; //max time bullet will be in scene for
    private float damage;

    void Start()
    {
        lifeSpan = 3.0f;
    }

    void Update()
    {
        lifeSpan -= Time.deltaTime;
        //destroy bullet after time is up
        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

    //bullet hit something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.collider.gameObject.GetComponent<Enemy>();
            enemy.hit(damage);
        }
        Destroy(gameObject);
    }

    //set damage of the bullet
    public void setDamage(float damage)
    {
        this.damage = damage;
    }
}
