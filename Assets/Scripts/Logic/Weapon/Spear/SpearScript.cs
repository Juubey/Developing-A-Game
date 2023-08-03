using UnityEngine;

public class SpearScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private HitMarker hitMarker;
    public GameObject impactEffect;

    bool hasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasHit == false)
        {
            trackTrajectory();
        }
    }

    void trackTrajectory()
    {
        Vector2 direction = rb.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //Applying the angle we just calculated
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        if(col.gameObject.CompareTag("Obstacle"))
        {
            Vector2 _wallNormal = col.contacts[0].normal;
            Vector2 direction = Vector2.Reflect(rb.velocity,_wallNormal).normalized;
            

        }
        if(col.gameObject.CompareTag("Enemy")) //if object hit has enemy tab show hitmarker
        {
            //Enemy enemy = other.GetComponent<Enemy>();
            //Debug.Log(other.name);
            
            //enemy.GetComponent<Enemy>().TakeDamage(damage);
            hitMarker.GetComponent<HitMarker>().HitEnable();
        }

        Instantiate(impactEffect, transform.position, transform.rotation);
        //Destroy(gameObject); //Destroys the projectile
    }
}
