using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool attack;
    public Transform attackBox;
    public int attackDamage = 30;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

void Start()
    {
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime){

        
            if(Input.GetKeyDown(KeyCode.Q))
            {
                //anim.SetTrigger("attack");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else
            {
                attack = false;
            }
        }
    }

    public void Attack()
    {
        attack = true;
        //animator.SetTrigger("Attack");
        //anim.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackBox.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmoSelected()
    {
        if (attackBox == null)
        return;

        Gizmos.DrawWireSphere(attackBox.position, attackRange);

    }
}
