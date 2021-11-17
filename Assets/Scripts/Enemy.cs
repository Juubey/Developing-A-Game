using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    int currentHealth;
    public int maxHealth = 100;
    public GameObject deathEffect;

    public Animator animator;
    public AIPath aiPath;

    void start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage (int damage){
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0){
            Die();
        }
    }   

    void update()
    {
        if(aiPath.desiredVelocity.x >= 0.0f)
        {
            transform.localScale =  new Vector3(-1f, 1f, 1f);

        }else if(aiPath.desiredVelocity.x <= -0.0f)
            {
                transform.localScale =  new Vector3(1f, 1f, 1f);
            }
        
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        //die animation
        animator.SetBool("Death", true);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        //disable enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject);
    }

}
