﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Used to control the storyline for the tutorial level
/// </summary>
public class Storyline_Tutorial : Storyline {

    public PlayerController Player1;
    public Player2Controller Player2;
    public List<MoleManContoller> MoleMen;
    public TorchFuelController TorchController;
    public List<GameObject> ReferencePoints;
    public GameObject CameraRig;

    private int _currentStep = 0;
    private bool _done = false;
    private ToolTips _tips;
    private bool _tipsDone = false;
    private float _startDisplay;


    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        _tips = new ToolTips();
        _tips.DisableToolTips();
      
	}


    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update() {
        if(Time.time - _startDisplay > 5 && !_tipsDone)
        {
            _tips.DisableToolTips();
        }
        if (_done)
        {
            return;
        }

        if(_currentStep == 0)
        {
            TorchController.IsDisabled = true;
            //Moleman walking to players
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            MoleMen[0].IsDisabled = false;
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
        }
        else if(_currentStep == 1)
        {
            //Moleman walks out
            _done = true;
            MoleMen[0].Next();
            TorchController.SwapPlayers();
            MoleMen[0].transform.Find("Spotlight").gameObject.SetActive(false);
            
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            MoleMen[0].IsDisabled = false;
            //Turn on keys for X seconds
            _tips.EnableToolTips();
            MoleMen[0].transform.Find("hips/Torch Light Holder").gameObject.SetActive(false);
            _startDisplay = Time.time;

        }
        else if (_currentStep == 2)
        {
            //Moleman walks out
            _done = true;
            MoleMen[0].IsDisabled = false;
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;


        }
    }

    /// <summary>
    /// Nexts this instance.
    /// </summary>
    public override void Next()
    {

    }

    /// <summary>
    /// Dialogues the complete.
    /// </summary>
    public override void DialogueComplete()
    {
        if(_currentStep == 0)
        {
            _currentStep++;
            _done = false;
        }

        else if (_currentStep == 1)
        {
            _currentStep++;
            _done = false;
        }
    }

    /// <summary>
    /// Moles the man in position.
    /// </summary>
    public override void NextMoleMan()
    {
        if(_currentStep == 1)
        {
            if(MoleMen.Count > 1)
            {
                MoleMen.RemoveAt(0);
            }
            
            _currentStep++;
        }
    }

    /// <summary>
    /// Disables the mole man.
    /// </summary>
    public override void DisableMoleMan()
    {
        MoleMen[0].IsDisabled = true;
    }

    /// <summary>
    /// Enables the mole man.
    /// </summary>
    public override void EnableMoleMan()
    {
        MoleMen[0].IsDisabled = false;
    }

}
