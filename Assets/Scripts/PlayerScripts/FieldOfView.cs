using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask enemyMask;

    private GameObject player;
    private Weapon weapon;
    
    //field of view mesh variables
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Bounds bounds;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    private int rayCount;
    private Vector3 origin;
    private float startingAngle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weapon = player.transform.GetChild(1).gameObject.GetComponent<Weapon>();

        //field of view mesh determined by weapon
        fov = weapon.getFov();
        viewDistance = weapon.getViewDistance();

        mesh = new Mesh();
        bounds = new Bounds(origin, Vector3.one * 1000.0f);
        GetComponent<MeshFilter>().mesh = mesh;

        //initialize field of view mesh
        rayCount = 40;
        origin = player.transform.position;
        startingAngle = player.transform.rotation.eulerAngles.z + fov * 0.5f;

        vertices = new Vector3[rayCount + 1 + 1];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];
    }

    void Update()
    {
        //update position and angle of field of view
        origin = player.transform.position;
        startingAngle = player.transform.rotation.eulerAngles.z + fov * 0.5f;

        float angle = startingAngle;
        float angleIncrement = fov / rayCount;
        vertices[0] = origin;

        //cast field of view rays
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            Vector3 vertex;

            //cast for obstacles (walls, crates, etc)
            RaycastHit2D obstacleRaycast = Physics2D.Raycast(origin, new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)), viewDistance, layerMask);
            //cast for enemies
            RaycastHit2D enemyRaycast = Physics2D.Raycast(origin, new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)), viewDistance, enemyMask);

            if (obstacleRaycast.collider == null)
            {
                //no hit       
                vertex = origin + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * viewDistance;
            }
            else
            {
                //hit object
                vertex = obstacleRaycast.point;
            }
            if (enemyRaycast.collider != null)
            {
                //Debug.Log(enemyRaycast.collider.gameObject.tag);
                if (enemyRaycast.collider.gameObject.tag == "Enemy")
                {
                    Enemy enemy = enemyRaycast.collider.gameObject.GetComponent<Enemy>();
                    enemy.makeVisible();
                }
                if (enemyRaycast.collider.gameObject.tag == "Bullet")
                {
                    Bullet bullet = enemyRaycast.collider.gameObject.GetComponent<Bullet>();
                    bullet.makeVisible();
                }

            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrement;
        }

        //update field of view mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = bounds;
    }

    //sets the fov variable
    public void setFov(float fov)
    {
        this.fov = fov;
    }

    //sets the viewDistance variable
    public void setViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }
}
