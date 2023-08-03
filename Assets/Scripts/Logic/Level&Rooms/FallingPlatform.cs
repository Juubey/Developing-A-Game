using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb2d; // Reference to our Rigidbody2D
    public float fallDelay;   // Amout of time until the fall begins
    private bool platformIsFall = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb2d.isKinematic = false;
        GetComponent<Collider2D>().isTrigger = true;
        platformIsFall = true;
        yield return 0;
    }
    void OnBecameInvisible()
    {
        if (platformIsFall) Destroy(gameObject);
    }
}
