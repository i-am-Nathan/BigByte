// file:	Assets\CutSceneTrigger.cs
//
// summary:	Implements the cut scene trigger class

﻿using UnityEngine;
using System.Collections;

/// <summary>   A cut scene trigger. </summary>
///
/// <remarks>    . </remarks>

public class CutSceneTrigger : MonoBehaviour
{
    /// <summary>   The story. </summary>
    public Storyline Story;
  
    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        
    }
      
    // Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        
    }

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="collider"> The collider. </param>

    void OnTriggerEnter(Collider collider)
    {
        Story.Next();
    }
}

