using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using SoulSpliter;

public class Player : MonoBehaviour, IDamageable
{

    public int maxHealth = 100;
    public int currentHealth;
    public int health = 100;
    public int level = 1;
    public Animator transition;
    public SpriteRenderer SpriteRend;
    public Material FlashMaterial;
    public Material OGMaterial;

    public HealthBar healthBar;
    public Movement Movement;
    public CameraShaker cameraShaker;
    public TimeFreezer timeFreezer;


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

        SpriteRend = gameObject.GetComponentInChildren<SpriteRenderer>();
        OGMaterial = SpriteRend.material;

    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(20);
        }
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            TakeHeal(20);
        }
        if (currentHealth == 0)
        {
            StartCoroutine(DeathDelay(0.1f));
        }
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
        StartCoroutine(DamageFlash());
        //gameObject.GetComponent<TimeFreezer>().FreezeTime(10.0f);
        gameObject.GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f);
        gameObject.GetComponent<CameraShaker>().BasicShake(5.0f, 5.0f);
    }
    public void TakeHeal(int heal)
    {
        currentHealth += heal;

        healthBar.SetHealth(currentHealth);
    }

    IEnumerator DeathDelay(float time)
    {
    isAlive = false;
    Movement.speed = 0;
    Movement.canMove = false;
    Movement.jumpForce = 0;
    //transition.SetTrigger("Start");
    //FindObjectOfType<GameManager>().EndGame();
    yield return new WaitForSeconds(time);

    }
    IEnumerator DamageFlash()
    {
        SpriteRend.material = FlashMaterial;
        yield return new WaitForSeconds(.25f);
        SpriteRend.material = OGMaterial;
    }
}
