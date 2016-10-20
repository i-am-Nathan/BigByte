﻿using UnityEngine;
using System.Collections;
  
public class CutSceneTrigger : MonoBehaviour
{
    public Storyline Story;
  
    // Use this for initialization
    void Start()
    {
        
    }
      
    // Update is called once per frame
    void Update()
    {
        
    }
 
    void OnTriggerEnter(Collider collider)
    {
        Story.Next();
    }
}

