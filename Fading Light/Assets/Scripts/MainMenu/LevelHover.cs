// file:	Assets\Scripts\MainMenu\LevelHover.cs
//
// summary:	Implements the level hover class

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>   A level hover. </summary>
///
/// <remarks>    . </remarks>

public class LevelHover : MonoBehaviour, IPointerEnterHandler
{
    /// <summary>   The level image. </summary>
    public Sprite LevelImage;
    /// <summary>   The image viewer. </summary>
    private Image _imageViewer;
	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
        _imageViewer = GameObject.FindGameObjectWithTag("LevelImage").GetComponent<Image>();
	}

    /// <summary>   <para></para> </summary>
    ///
 
    ///
    /// <param name="eventData">    Current event data. </param>

    public void OnPointerEnter(PointerEventData eventData)
    {
        _imageViewer.sprite = LevelImage;
    }

}
