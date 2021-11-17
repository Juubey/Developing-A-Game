using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public int health = 100;
    public int level = 1;
    public Animator transition;
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

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
            if(currentHealth == 0){
            transition.SetTrigger("Start");
            FindObjectOfType<GameManager>().EndGame();
            
            }
        }
    }

    void TakeDamage(int damage){
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
