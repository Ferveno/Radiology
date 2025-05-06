using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth; // Current health of the player
    public float damageCooldown = 1.0f; // Cooldown duration in seconds
    private bool isOnCooldown = false; // Cooldown state

    public Slider healthSlider; // Reference to the UI Slider
    public TextMeshProUGUI playerLiveCounter_Text;

    public int playerLive = 1;

    public float regenAmount = 5f; // Amount of health to regenerate per tick
    public float regenCooldown = 5.0f; // Time in seconds after last damage to start regeneration
    public float regenInterval = 1.0f; // Interval between each regeneration tick
    public float regenSpeed = 1f; // Speed of health bar filling smoothly

    private Coroutine regenCoroutine; // Reference to the regeneration coroutine

    void Start()
    {
        UpdatePlayerLive();

        currentHealth = maxHealth; // Initialize current health
        healthSlider.maxValue = maxHealth; // Set the maximum value of the slider
        healthSlider.value = currentHealth; // Initialize the slider's value
    }

    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        if (!isOnCooldown)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }

            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine); // Stop regeneration when taking damage
            }

            StartCoroutine(DamageCooldown());
        }
    }

    // Update the health slider
    public void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }

    // Method to handle player death
    private void Die()
    {
        Debug.Log("Player Died!");
        // Handle player death (e.g., reload the scene, show game over screen, etc.)

        if (playerLive > 1)
        {
            UpdatePlayerLive(-1);
            currentHealth = maxHealth;
            UpdateHealthUI();
        }
        else
        {
            //StartCoroutine(SoundManager.instance.ChangeBGMusicByFade(SoundManager.instance.gameOver_Music));

            if(SoundManager.instance != null)
            {
                SoundManager.instance.asrc_Music.Stop();
                SoundManager.instance.Play("GameOver");
            }
          
            Time.timeScale = 0f;
            GameOverPanel_UI.ShowUI();
        }
    }

    // Method to be called when the player is healed
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        StartCoroutine(SmoothHealthBarUpdate());
    }

    // Coroutine to handle the damage cooldown
    private IEnumerator DamageCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(damageCooldown);
        isOnCooldown = false;
        regenCoroutine = StartCoroutine(HealthRegeneration()); // Start regeneration after cooldown
    }

    // Coroutine to handle health regeneration
    private IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(regenCooldown); // Wait for the regeneration cooldown

        while (currentHealth < maxHealth)
        {
            float targetHealth = currentHealth + regenAmount;
            if (targetHealth > maxHealth)
            {
                targetHealth = maxHealth;
            }

            float elapsedTime = 0f;
            float startHealth = currentHealth;

            while (elapsedTime < regenInterval)
            {
                elapsedTime += Time.deltaTime * regenSpeed;
                currentHealth = Mathf.Lerp(startHealth, targetHealth, elapsedTime / regenInterval);
                UpdateHealthUI();
                yield return null;
            }

            currentHealth = targetHealth;
            UpdateHealthUI();

            if (currentHealth >= maxHealth)
            {
                break;
            }

            yield return new WaitForSeconds(regenInterval - elapsedTime / regenSpeed);
        }
    }

    private IEnumerator SmoothHealthBarUpdate()
    {
        float elapsedTime = 0f;
        float startHealth = healthSlider.value;
        float targetHealth = currentHealth;

        while (elapsedTime < regenInterval)
        {
            elapsedTime += Time.deltaTime * regenSpeed;
            healthSlider.value = Mathf.Lerp(startHealth, targetHealth, elapsedTime / regenInterval);
            yield return null;
        }

        healthSlider.value = targetHealth;
    }

    public void UpdatePlayerLive(int live = 0)
    {
        playerLive += live;
        playerLiveCounter_Text.text = "x" + (playerLive - 1).ToString();

        if (playerLive > 1)
        {
            playerLiveCounter_Text.gameObject.SetActive(true);
        }
        else
        {
            playerLiveCounter_Text.gameObject.SetActive(false);
        }
    }

    public void IncreaseSpeedHealthGenerationSpeed(float speedToAdd)
    {
        regenSpeed += speedToAdd;
    }
}
