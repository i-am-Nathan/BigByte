// file:	assets\scripts\shop&collectables\shopkeeper.cs
//
// summary:	Implements the shopkeeper class

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class is used for the shop. Players can interact with the shopkeeper and purchase items
/// using the gold they have accumulated during gameplay.
/// </summary>
///
/// <remarks>    . </remarks>

public class ShopKeeper : MonoBehaviour {
    /// <summary>   The main camera. </summary>
	public Camera MainCamera;
    /// <summary>   The shop keeper camera. </summary>
	public Camera ShopKeeperCamera;
    /// <summary>   True to shopping. </summary>
	private bool _shopping;
    /// <summary>   The transition. </summary>
	private Animation _transition;
    /// <summary>   The first player. </summary>
	public PlayerController Player1;
    /// <summary>   The second player. </summary>
	public Player2Controller Player2;
    /// <summary>   The item stand. </summary>
	public GameObject ItemStand;
    /// <summary>   True if this object has played. </summary>
	private bool _hasPlayed;
    /// <summary>   The animator. </summary>
    private Animator _animator;

    /// <summary>   Manager for sub inventory. </summary>
	public SubInventoryManager SubInventoryManager;

    /// <summary>   Used to set up the shop animation. </summary>
    ///
 

    void Awake(){
		ShopKeeperCamera.enabled = false;
		_transition = ShopKeeperCamera.GetComponent<Animation> ();
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>   Setting up the item stand. </summary>
    ///
 

	void Start(){
		_hasPlayed = false;
		ItemStand.SetActive (false);
    }

    /// <summary>   Playing the animation to rotate the potions. </summary>
    ///
 

	void Update(){
		if (!_transition.isPlaying && _hasPlayed) {
			ItemStand.SetActive (true);
			StartCoroutine(DelayAnimation());

		} 
	}

    /// <summary>   Plays the animation for the moleman at intervals. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	private IEnumerator DelayAnimation()
	{
		int rand = Random.Range (1, 3);
		if (rand == 1) {
			_animator.SetTrigger("Scratch");
			yield return new WaitForSeconds(4f);
			_animator.ResetTrigger ("Scratch");
		} else {
			_animator.SetTrigger("Taunt");
			yield return new WaitForSeconds(4f);
			_animator.ResetTrigger ("Taunt");
		}

	
	}

    /// <summary>
    /// When players press Q or O when they are collding with the shopkeeper, it will open the shop.
    /// </summary>
    ///
 
    ///
    /// <param name="other">    Other. </param>

	void OnTriggerStay(Collider other){
        if (other.name == "Player 1")
        {
            if (Input.GetKeyDown(KeyCode.O) && !_shopping)
            {
                var _achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag("AchievementManager").GetComponent(typeof(AchievementManager));
                _achievementManager.AchievementObtained("Shopping Spree!");

                _shopping = true;
                SubInventoryManager.ToggleInShop();
                Player1.IsDisabled = true;
                Player2.IsDisabled = true;
                MainCamera.enabled = false;
                ShopKeeperCamera.enabled = true;
                _transition.Play();
                _hasPlayed = true;

            }
            else if (Input.GetKeyDown(KeyCode.O) && _shopping)
            {
                _shopping = false;
                SubInventoryManager.ToggleInShop();
                _hasPlayed = false;
                _transition.Stop();
                MainCamera.enabled = true;
                ShopKeeperCamera.enabled = false;
                Player1.IsDisabled = false;
                Player2.IsDisabled = false;
                ItemStand.SetActive(false);
            }
        }
        else if (other.name == "Player2")
        {
            if (Input.GetKeyDown(KeyCode.Q) && !_shopping)
            {
                var _achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag("AchievementManager").GetComponent(typeof(AchievementManager));
                _achievementManager.AchievementObtained("Shopping Spree!");
                _shopping = true;
                SubInventoryManager.ToggleInShop();
                Player1.IsDisabled = true;
                Player2.IsDisabled = true;
                MainCamera.enabled = false;
                ShopKeeperCamera.enabled = true;
                _transition.Play();
                _hasPlayed = true;

            }
            else if (Input.GetKeyDown(KeyCode.Q) && _shopping)
            {
                _shopping = false;
                SubInventoryManager.ToggleInShop();
                _hasPlayed = false;
                _transition.Stop();
                MainCamera.enabled = true;
                ShopKeeperCamera.enabled = false;
                Player1.IsDisabled = false;
                Player2.IsDisabled = false;
                ItemStand.SetActive(false);
            }
        }
	}
}