using UnityEngine;
using System.Collections;

/// <summary>
/// Coin objects which players can pick up and purchase items through the shop.
/// </summary>
public class Coins : MonoBehaviour
{
    public AudioClip PickUpSound;
    private AudioSource _source;
    private bool _notPickedUp;
    private GameData _gameDataScript;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _notPickedUp = true;
    }


    /// <summary>
    /// This will load up the player objects so that when coins are picked up, they will go to the respective player.
    /// </summary>
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Game Data");
        _gameDataScript = (GameData)go.GetComponent(typeof(GameData));
    }

    /// <summary>
    /// When player collides with the coin, they will increment the player's gold and play a sound when picked up.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter(Collider other)
    {
        if (_notPickedUp)
        {
            _notPickedUp = false;
            _source.PlayOneShot(PickUpSound);
            GetComponent<Renderer>().enabled = false;
            Destroy(gameObject, PickUpSound.length + 0.1f);

            _gameDataScript.UpdateGold(1);
        }
    }
}