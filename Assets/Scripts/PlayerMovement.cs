using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f; //player movement speed
    [SerializeField] private float walkMultiplier = 0.5f; //silent movement/walk multiplier
    [SerializeField] private float movingVolume = 5.0f; //noise player makes while moving/running

    private bool moving;
    private bool walking;
    private Vector3 mousePosition;
    private Camera camera;
    private Rigidbody2D body;

    void Start()
    {
        moving = false;
        walking = false;
        body = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    void Update()
    {
        move();
        rotatePlayer();
    }

    //moves the player
    void move()
    {
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            walking = true;
            moving = false;
            StopCoroutine("moveNoise");
            speed *= walkMultiplier;
        }
        else
        {
            walking = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (!moving && !walking)
            {
                moving = true;
                StartCoroutine("moveNoise");
            }
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!moving && !walking)
            {
                moving = true;
                StartCoroutine("moveNoise");
            }
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!moving && !walking)
            {
                moving = true;
                StartCoroutine("moveNoise");
            }
            transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!moving && !walking)
            {
                moving = true;
                StartCoroutine("moveNoise");
            }
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            moving = false;
            StopCoroutine("moveNoise"); //stop making noise when player stops moving
        }
    }

    //sets the moving variable of the player
    public void setMoving(bool moving)
    {
        this.moving = moving;
    }

    //gets the moving variable of the player
    public bool getMoving()
    {
        return moving;
    }

    //rotates the player in the direction of the mouse
    void rotatePlayer()
    {
        mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - camera.transform.position.z));
        body.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg);
    }

    //routine for making noise when the player is moving
    private IEnumerator moveNoise()
    {
        //continue checking while the player is still moving
        for (; ; )
        {
            audioRadius(movingVolume); //check if any enemies heard the noise
            yield return new WaitForSeconds(0.2f); //wait time is for time between steps
        }
    }

    //check for enemies within range to hear the player making noise
    public void audioRadius(float soundRadius)
    {
        //create contact filter to only check for enemies
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Enemies");

        //check for enemies within radius
        Collider2D[] colliders = new Collider2D[20];
        int enemiesFound = Physics2D.OverlapCircle(transform.position, soundRadius, filter, colliders);
        Debug.Log(enemiesFound.ToString() + " enemies heard the player moving:");

        //alert each enemy of player's position
        foreach (Collider2D c in colliders)
        {
            if (!(c is null))
            {
                Debug.Log(c);
                Enemy e = c.gameObject.GetComponent<Enemy>();
                e.setHeard(true, transform.position);
            }
            else
            {
                break;
            }
        }
    }
}
