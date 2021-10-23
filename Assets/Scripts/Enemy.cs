using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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

    //makes this enemy object visible while in player's field of view
    public void makeVisible()
    {
        Debug.Log("enemy visible");
        spriteRenderer.enabled = true;
        timer = 0.05f;
    }

}
