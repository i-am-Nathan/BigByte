using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Storyline_Level3 : Storyline
{


    public PlayerController Player1;
    public Player2Controller Player2;
    public List<MoleManContoller> MoleMen;
    public TorchFuelController TorchController;
    public List<GameObject> ReferencePoints;
    public GameObject CameraRig;
    public PrisonerController Prisoner;
    public SkeleBoss Boss;
    public List<GameObject> CutScenePositions;
    public List<GameObject> CustSceneTargets;

    public int _currentStep = 0;
    private bool _done = false;
    private ToolTips _tips;
    private bool _tipsDone = false;
    private float _startDisplay;
    public GameObject Block;

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
        if (_currentStep == 3)
        {
            _done = false;
            _currentStep++;
        }

        //throw new NotImplementedException();
    }

    public override void NextMoleMan()
    {
        if (_currentStep == 4)
        {
            _done = false;
            _currentStep++;
        }
        //throw new NotImplementedException();
    }

    public override void StartText()
    {
        if(_currentStep == 0)
        {
            _currentStep++;
            _done = false;
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

        if(_currentStep == 0)
        {
            TorchController.SwapPlayers();
            _done = true;
        }
        else if(_currentStep == 1)
        {
            //zoom in on prisoner
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[0];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[0];
            TorchController.IsDisabled = true;
            //Moleman walking to players
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
        }
        else if (_currentStep == 2)
        {
            //prisoner running away
            Prisoner.IsDisabled = false;
        }
        else if (_currentStep == 3)
        {
            //zoom back out
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            DestroyObject(Block);
        }else if (_currentStep == 4)
        {
            TorchController.IsDisabled = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[1];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[1];
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
            Boss.BeginCutscene(this);
            CharacterDamageEnabled(false);
            _done = true;
            _done = true;
        }
        else if (_currentStep == 5)
        {
            //zoom back out
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
        }


    }

    public override void CharacterDamageEnabled(bool enabled)
    {
        Player1.CanTakeDamage = enabled;
        Player2.CanTakeDamage = enabled;
    }
}
