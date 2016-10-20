using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class is used for the shop. Players can interact with the shopkeeper and purchase items using the gold they have 
/// accumulated during gameplay.
/// </summary>
public class ShopKeeper : MonoBehaviour {
	public Camera MainCamera;
	public Camera ShopKeeperCamera;
	private bool _shopping;
	private Animation _transition;
	public PlayerController Player1;
	public Player2Controller Player2;
	public GameObject ItemStand;
	private bool _hasPlayed;
    private Animator _animator;

	public SubInventoryManager SubInventoryManager;

    void Awake(){
		ShopKeeperCamera.enabled = false;
		_transition = ShopKeeperCamera.GetComponent<Animation> ();
        _animator = GetComponentInChildren<Animator>();//need this...
    }
		
	void Start(){
		_hasPlayed = false;
		ItemStand.SetActive (false);
    }

	void Update(){
		if (!_transition.isPlaying && _hasPlayed) {
			ItemStand.SetActive (true);
			StartCoroutine(DelayAnimation());

		} 

	}

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
	/// <param name="other">Other.</param>
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