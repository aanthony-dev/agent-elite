using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private float reactionTime = 0.5f; //how long before the enemy will react to the player

    private Rigidbody2D body;
    private Enemy e;
    private AudioSource runNoise;
    private bool moving;

    void Start()
    {
        e = GetComponent<Enemy>();
        body = GetComponent<Rigidbody2D>();
        runNoise = transform.GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        moving = false;
        runNoise.volume = 0;
    }

    void Update()
    {
        if (e.getHeard())
        {
            runNoise.volume = 1;
        }

        //go investigate the noise
        if (e.getHeard() && !e.getCanSeePlayer()) 
        {
            agent.SetDestination(e.getLastHeardPosition());
            moving = true;
            StartCoroutine("reaction"); //delay turning reaction toward noise
        }

        //moved to last heard location and have not found player
        if (agent.remainingDistance <= 0.5f && !e.getCanSeePlayer())
        {
            e.setHeard(false);
            moving = false;
        }

        //look at the player
        else if (e.getCanSeePlayer())
        {
            agent.SetDestination(transform.position);
            moving = false;
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = angle;
        }

        //go investigate where player was last seen
        else if (e.getSawPlayer() && !e.getCanSeePlayer())
        {
            agent.SetDestination(e.getLastSeenPosition());
            moving = true;
            Vector2 currentPosition = transform.position;
            Vector3 direction = e.getLastSeenPosition() - currentPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = angle;
        }
        
    }

    //introduces a reaction time delay for the enemy when turning
    private IEnumerator reaction()
    {

        //reaction time delay
        for (float time = reactionTime; time >= 0.0f; time -= Time.deltaTime)
        {
            yield return null;
        }

        //adjust body rotation
        Vector2 currentPosition = transform.position;
        Vector3 direction = e.getLastHeardPosition() - currentPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        body.rotation = angle;
    }
}
