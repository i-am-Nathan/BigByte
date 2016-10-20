﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Storyline_Level2 : Storyline
{


    public PlayerController Player1;
    public Player2Controller Player2;
    public List<MoleManContoller> MoleMen;
    public TorchFuelController TorchController;
    public List<GameObject> ReferencePoints;
    public GameObject CameraRig;

    public List<GameObject> CutScenePositions;
    public List<GameObject> CustSceneTargets;

    public int _currentStep = 0;
    private bool _done = false;
    private ToolTips _tips;
    private bool _tipsDone = false;
    private float _startDisplay;

    public override void DialogueComplete()
    {
        _currentStep++;
        _done = false;
       // throw new NotImplementedException();
    }

    public override void EnableMoleMan()
    {
        //throw new NotImplementedException();
    }

    public override void Next()
    {
       

        //throw new NotImplementedException();
    }

    public override void NextMoleMan()
    {
        //throw new NotImplementedException();
    }

    public override void StartText()
    {
        if (_currentStep == 0)
        {
            _done = false;
            _currentStep++;
        }
        else if (_currentStep == 2)
        {
            _done = false;
            _currentStep++;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (_done)
        {
            return;
        }

        if (_currentStep == 0)
        {
            TorchController.SwapPlayers();
            MoleMen[1].IsDisabled = true;
            _done = true;
        }
        else if (_currentStep == 1)
        {

            Debug.Log("Step 2");
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[0];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[0];
            TorchController.IsDisabled = true;
            //Moleman walking to players
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            MoleMen[0].IsDisabled = false;
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
            _done = true;
        }
        else if(_currentStep == 2)
        {
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
        }
        else if (_currentStep == 3)
        {
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[1];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[1];
            TorchController.IsDisabled = true;
            //Moleman walking to players
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            MoleMen[1].IsDisabled = true;
            MoleMen[1].Speed = 20;
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
            _done = true;
        }
        else if (_currentStep == 4)
        {
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
        }
    }

    public override void CharacterDamageEnabled(bool enabled)
    {
        Player1.CanTakeDamage = enabled;
        Player2.CanTakeDamage = enabled;
    }
}
