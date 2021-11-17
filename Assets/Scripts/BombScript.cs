using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public GameObject explosionEffect;
    public float force = 700f;
    public LayerMask LayerToHit;

    float countdown;
    bool hasExploded = false;

    void start (){
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        GameObject explosionEffectIns = Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] collidors = Physics2D.OverlapCircleAll(transform.position, radius, LayerToHit);

        foreach (Collider2D nearbyObject in collidors)
        {
            Vector2 direction = nearbyObject.transform.position - transform.position;
            //nearbyObject.GetComponent<Rigidbody2D>().AddForce(direction * force);

            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.AddForce(direction * force);
            }
        }
        Destroy(gameObject);
        Destroy(explosionEffectIns, 10);
        Debug.Log("Boom!");
    }
}
