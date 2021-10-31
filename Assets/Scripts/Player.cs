using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health = 10.0f;

    private Weapon weapon;

    void Awake()
    {
        //give weapon to player
        Instantiate(Resources.Load("Rifle") as GameObject, transform.position, transform.rotation).transform.parent = transform;
    }

    void Start()
    {
        weapon = transform.GetChild(1).gameObject.GetComponent<Weapon>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Gadget Used");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.reload();
            Debug.Log("Reloading");
        }
    }

    //subtracts bullet damage from health and destroys if out of health.
    public void hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("You Died");
            Destroy(gameObject);
        }
    }
}
