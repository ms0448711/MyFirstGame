using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthContrller : MonoBehaviour
{
    public static PlayerHealthContrller instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float invincCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = CharacterTracker.instace.maxHealth;
        currentHealth = CharacterTracker.instace.currentHealth;


        //currentHealth = maxHealth;

        UIController.instace.healthSlider.maxValue = maxHealth;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;

            if (invincCounter <= 0)
            {
                SetBodyInvinc(false);
            }
        }
    }

    public void DamagePlayer()
    {

        if (invincCounter <= 0)
        {
            AudioManager.instance.PlaySFX(AudioManager.AudioCode.PlayerHurt);

            currentHealth--;

            MakeInvincible(damageInvincLength);

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);

                UIController.instace.deathScreen.SetActive(true);

                AudioManager.instance.PlayGameOver();
                AudioManager.instance.PlaySFX(AudioManager.AudioCode.PlayerDeath);
            }

            UpdateHealth();
        }
    }

    private void UpdateHealth()
    {
        UIController.instace.healthSlider.value = currentHealth;
        UIController.instace.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    private void SetBodyInvinc(bool isInvinc)
    {
        Color bodyColor = PlayerController.instance.bodySR.color;
        float r = bodyColor.r, g = bodyColor.g, b = bodyColor.b;
        if (isInvinc)
            PlayerController.instance.bodySR.color = new Color(r, g, b, .45f);
        else
            PlayerController.instance.bodySR.color = new Color(r, g, b, 1f);
    }

    public void MakeInvincible(float length)
    {
        invincCounter = length;
        SetBodyInvinc(true);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        UpdateHealth();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;

        UIController.instace.healthSlider.maxValue = maxHealth;
        UpdateHealth();
    }
}
