using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Projectile;
    public float ProjectileForce = 0;

    Vector2 direction;
    public float force;
    public GameObject PointPrefab;
    public GameObject[] points;

    public int numberOfPoints;
    public float throwSpeedMult = 4.0f;

    void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(PointPrefab, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 launcherPos = transform.position;

        direction = MousePos - launcherPos; // Calc the direction

        

        FaceMouse();

        for (int i = 0; i < points.Length; i++)
        {
            points[i].transform.position = pointPosition(i * 0.1f);
        }



        if(Input.GetButtonDown("Fire2") || !Input.GetButtonUp("Fire2"))
            {
                if(ProjectileForce < 100){                
                ProjectileForce += Time.deltaTime * throwSpeedMult;
                
                //Debug.Log(ProjectileForce);
                }
                
            }
        if(Input.GetButtonUp("Fire2"))
        {
            Shoot();
            ProjectileForce = 0;
        }
        
    }

    void Shoot()
    {
        GameObject ProjectileIns = Instantiate(Projectile, transform.position, transform.rotation);

        //ProjectileIns.GetComponent<Rigidbody2D>().AddForce(transform.right * ProjectileForce);
        ProjectileIns.GetComponent<Rigidbody2D>().velocity = transform.right * ProjectileForce;
    }

    void FaceMouse()
    {
        transform.right = direction;
    }

    Vector2 pointPosition(float t)
    {
        Vector2 currentPosition = (Vector2)transform.position + (direction.normalized * force * t) + 0.5f * Physics2D.gravity * (t*t);

        return currentPosition;
    }
    
    
}
