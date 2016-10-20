using UnityEngine;
using System.Collections;

public class SpearTrap : MonoBehaviour {
    public AudioSource SpearSound;
    public float SpearSpeed = 3f;
    private Animation anim;
	// Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        StartCoroutine(SpearAnim());
    }
	
    void RepeatCoroutine()
    {
        StartCoroutine(SpearAnim());
    }

    private IEnumerator SpearAnim()
    {
        anim.Play();
        SpearSound.Play();
        yield return new WaitForSeconds(SpearSpeed);
        RepeatCoroutine();
        yield return null;
    }
}
