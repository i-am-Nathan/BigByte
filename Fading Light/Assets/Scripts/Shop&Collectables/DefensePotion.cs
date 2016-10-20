using UnityEngine;
using System.Collections;

/// <summary>
/// Defense potions which players can pick up and purchase items through the shop.
/// </summary>
public class DefensePotion : MonoBehaviour
{
	// Audio source
    private AudioSource _source;
    public AudioClip PickUpSound;
    
    private bool _notPickedUp;
    private GameData _gameDataScript;

	/// <summary>
	/// Called to obtain the audio source
	/// </summary>
    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _notPickedUp = true;
    }


    /// <summary>
    /// This will load up the player objects so that when potions are picked up, they will go to the respective player.
    /// </summary>
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Game Data");
        _gameDataScript = (GameData)go.GetComponent(typeof(GameData));
    }

    /// <summary>
    /// When player collides with the potion, they will increment the player's number of potions and play a sound when picked up.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
		// Checking if a player has picked up the potion
		if (_notPickedUp && (other.tag == "Player" || other.tag == "Player2"))
        {
			// Destroying the potion while playing the sounds
            _notPickedUp = false;
            _source.PlayOneShot(PickUpSound);
            GetComponent<Renderer>().enabled = false;
            Destroy(gameObject, PickUpSound.length + 0.1f);

			// Updating the respective players sub-inventory
            if (other.tag == "Player")
            {
                SubInventoryManager SubInventoryManager = GameObject.Find("SubInventoryManager").GetComponent<SubInventoryManager>();
				SubInventoryManager.AddItemQuantity("Defense Potion", true);

            } else
            {
                SubInventoryManager SubInventoryManager = GameObject.Find("SubInventoryManager").GetComponent<SubInventoryManager>();
				SubInventoryManager.AddItemQuantity("Defense Potion", false);
            }
        }
    }
}