using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player1Health : MonoBehaviour {

    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Image healthCircle;                                 // Reference to the UI's health bar.


    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.

    void Awake()
    {
        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update()
    {
        TakeDamage(1);
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            Debug.Log("took damage");
        }
        
        // Reset the damaged flag.
        damaged = false;
    }


    void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthCircle.fillAmount -= amount/100.0f;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;
        Debug.Log("Dead");
    }
}
