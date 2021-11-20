using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] protected Transform target;
    [SerializeField] protected NavMeshAgent agent;

    private Rigidbody2D body;
    private Enemy e;

    void Start()
    {
        e = GetComponent<Enemy>();
        body = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        //go investigate the noise
        if (e.getHeard()) 
        {
            //agent.SetDestination(target.position);
            agent.SetDestination(e.getNoiseLocation());
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = angle;
        }

        //look at the player
        else if (e.getCanSeePlayer())
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = angle;
        }
        
    }
}
