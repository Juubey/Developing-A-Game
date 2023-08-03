using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    public TMP_Text healthText;
    public Image healthBar;

    float health, maxHealth = 100;
    float lerpSpeed;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        healthText.text = health + "%";
        if (health > maxHealth) health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            Damage(20f);
        }
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));

        healthBar.color = healthColor;

    }

    public void Damage(float damagePoints)
    {
        if (health > 0)
            health -= damagePoints;
    }
    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
            health += healingPoints;

    }
}