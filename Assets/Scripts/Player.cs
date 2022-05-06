using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public int health = 100;
    public int level = 1;
    public Animator transition;

     public HealthBar healthBar;
     public Movement Movement;

    [Space]
    [Header("Booleans")]
    public bool isAlive = true;

    public void SavePlayer (){
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer(){
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        currentHealth = data.health;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

    }

   

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(20);
            if(currentHealth == 0){           
            StartCoroutine(DeathDelay(0.1f));
            }
        }
    }

    void TakeDamage(int damage){
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    IEnumerator DeathDelay(float time)
    {
    isAlive = false;
    Movement.speed = 0;
    Movement.canMove = false;
    Movement.jumpForce = 0;
    transition.SetTrigger("Start");
    FindObjectOfType<GameManager>().EndGame();
    yield return new WaitForSeconds(time);

    }
}
