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
        if (e.getHeard() && !e.getCanSeePlayer()) 
        {
            agent.SetDestination(e.getLastHeardPosition());
            Vector2 currentPosition = transform.position;
            Vector3 direction = e.getLastHeardPosition() - currentPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = angle;
        }

        //moved to last heard location and have not found player
        if (agent.remainingDistance <= 0.5f && !e.getCanSeePlayer())
        {
            e.setHeard(false);
        }

        //look at the player
        else if (e.getCanSeePlayer())
        {
            agent.SetDestination(transform.position);
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = angle;
        }

        //go investigate where player was last seen
        else if (e.getSawPlayer() && !e.getCanSeePlayer())
        {
            agent.SetDestination(e.getLastSeenPosition());
            Vector2 currentPosition = transform.position;
            Vector3 direction = e.getLastSeenPosition() - currentPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = angle;
        }
        
    }
}
