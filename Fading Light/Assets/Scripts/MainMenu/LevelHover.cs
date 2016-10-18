using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelHover : MonoBehaviour, IPointerEnterHandler
{
    public Sprite LevelImage;
    private Image _imageViewer;
	// Use this for initialization
	void Start () {
        _imageViewer = GameObject.FindGameObjectWithTag("LevelImage").GetComponent<Image>();
	}
	
    public void OnPointerEnter(PointerEventData eventData)
    {
        _imageViewer.sprite = LevelImage;
    }

}
