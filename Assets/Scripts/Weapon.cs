using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public float hitRadius = 0.5f;
    public GameObject bulletPrefab;
    private PlayerCombat enemyLayers;
    Rigidbody2D rb;
    public GameObject impactEffect;
    public GameObject Spear;
    public int damage = 30;
    public float speed = 20f;
    [SerializeField] private Transform tomeTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2"))
        {
            Shoot();
            trackTrajectory();
        }
        if(Input.GetButton("Fire1"))
        {
            tomeTransform.position = transform.position;
            Vector3 throwDir = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;
            float throwForce = 1000.0f;
            tomeTransform.GetComponent<Rigidbody2D>().AddForce(throwDir * throwForce, ForceMode2D.Impulse);
            
        }
    }

    void trackTrajectory()
    {
        Vector2 direction = rb.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //Applying the angle we just calculated
    }

    void onTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    void OnBecameInvisible()
    {
        enabled = false;
        Destroy(gameObject);
    }

    void Shoot()
    {
        //shooting logic
        GameObject bulletIns = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletIns.GetComponent<Rigidbody2D>().velocity = transform.right * speed;


        GameObject spearIns = Instantiate(Spear, transform.position, transform.rotation);
        spearIns.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, hitRadius/*, enemyLayers*/);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
        }
    }
}
