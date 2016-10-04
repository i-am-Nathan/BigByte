using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public int startingHealth;
    public int currentHealth;
    public Slider healthSlider;

    PlayerController playerController;
    bool isDead;
    bool damaged;
	// Use this for initialization
	void Awake () {
        playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if(currentHealth <=0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        playerController.enabled = false;
    }
}
