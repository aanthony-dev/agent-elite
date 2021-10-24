using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f; //player movement speed

    private bool moving;
    private Vector3 mousePosition;
    private Camera camera;
    private Rigidbody2D body;

    void Start()
    {
        moving = false;
        body = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    void Update()
    {
        if (moving)
        {
            move();
        }
        isMoving();
        rotatePlayer();
    }

    //moves the player
    void move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    //updates the moving variable depending on if the player is moving or not
    void isMoving()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            moving = true;
        }
        else
        {
            moving = false;
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
}
