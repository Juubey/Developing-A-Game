using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100;
    public GameObject deathEffect;

    public Animator animator;
    public AIPath aiPath;
    public EnemyHealthBarBehavior Healthbar;

    void Start()
    {
        currentHealth = maxHealth;
        Healthbar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage (float damage){

        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
            
        //animator.SetTrigger("Hurt");

        if(currentHealth <= 0){
            Die();
        }
    }   

    void Update()
    {
        if(Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage(100f);
            Debug.Log("Enemy is at " + currentHealth);
        }
        Healthbar.SetHealth(currentHealth, maxHealth);
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        //die animation
        //animator.SetBool("Death", true);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        //disable enemy
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
        Destroy(gameObject);
    }

}
