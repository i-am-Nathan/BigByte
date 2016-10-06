using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Used to keep track of the health of a player
/// </summary>
public class PlayerHealth : MonoBehaviour {

	// Health attributes of the player including the Slider indicating their current health
    public int startingHealth;
    public int currentHealth;
    bool isDead;
    bool damaged;
	public Slider healthSlider;
	PlayerController playerController;

	/// <summary>
	/// Used for initialisation
	/// </summary>
	void Awake () {
		// Getting the player controller
        playerController = GetComponent<PlayerController>();
	}

	/// <summary>
	/// Used to decrease the health bar to represent the players health when hit
	/// </summary>
    public void TakeDamage(int amount)
    {
		// Decreasing their current health and setting the slider accordingly
        currentHealth -= amount;
        healthSlider.value = currentHealth;

		// Checking if the player is dead
        if(currentHealth <=0 && !isDead)
        {
            Death();
        }
    }

	/// <summary>
	/// Used when a player is dead
	/// </summary>
    void Death()
    {
		// Setting the player controller
        isDead = true;
        playerController.enabled = false;
    }
}
