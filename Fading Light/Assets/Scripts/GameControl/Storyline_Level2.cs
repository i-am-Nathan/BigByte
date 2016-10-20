using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Storyline class for the second level
/// </summary>
public class Storyline_Level2 : Storyline
{
	// Cutscenes and other game objects to be used as part of the storyline
    public PlayerController Player1;
    public Player2Controller Player2;
    public List<MoleManContoller> MoleMen;
    public TorchFuelController TorchController;
    public List<GameObject> ReferencePoints;
    public GameObject CameraRig;
    public MoleDoggy Boss;
    public List<GameObject> CutScenePositions;
    public List<GameObject> CustSceneTargets;

    public int _currentStep = 0;
    private bool _done = false;
    private ToolTips _tips;
    private bool _tipsDone = false;
    private float _startDisplay;

	/// <summary>
	/// Called when dialogue is complete
	/// </summary>
    public override void DialogueComplete()
    {
        if (_currentStep == 1)
        {
            _currentStep++;
            _done = false;
        }
        else if (_currentStep == 3)
        {
            _currentStep++;
            _done = false;
        }
        // throw new NotImplementedException();
    }

	/// <summary>
	/// Activating the moleman
	/// </summary>
    public override void EnableMoleMan()
    {
        //throw new NotImplementedException();
    }

	/// <summary>
	/// Moving to the next lines in the dialogue
	/// </summary>
    public override void Next()
    {
        if(_currentStep == 4)
        {
            _currentStep++;
            _done = false;
        }
       
    }

	/// <summary>
	/// Activating the next moleman in the scene
	/// </summary>
    public override void NextMoleMan()
    {
        if (_currentStep == 5)
        {
            _currentStep++;
            _done = false;
        }
        //throw new NotImplementedException();
    }

	/// <summary>
	/// Called near the start of the level
	/// </summary>
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
	
	/// <summary>
	/// Used to make the moleman walk and also to enable cutscenes based on the current steps
	/// </summary>
	void Update () {
        if (_done)
        {
            return;
        }

		// 0 means the start of the dialog (near the start of the level)
        if (_currentStep == 0)
        {
            TorchController.SwapPlayers();
            MoleMen[1].IsDisabled = true;
            _done = true;
        }
        else if (_currentStep == 1)
        {
			// Moving camera
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[0];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[0];
            TorchController.IsDisabled = true;
            //Moleman walking to players
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            MoleMen[1].IsDisabled = true;
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
            _done = true;
        }
        else if(_currentStep == 2)
        {
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            _done = true;
        }
        else if (_currentStep == 3)
        {
			// Moving camera and enabling cutscene
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
            _done = true;
        }
        else if (_currentStep == 5)
        {
			// Cutscene
            TorchController.IsDisabled = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[2];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[2];
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
            Boss.BeginCutscene(this);
            CharacterDamageEnabled(false);
            _done = true;
        }
        else if (_currentStep == 6)
        {
			// If at end of level
            TorchController.IsDisabled = false;
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            CharacterDamageEnabled(true);
            _done = true;
        }
    }

	/// <summary>
	/// Enabling the character damage (as it is disabled during cutscenes)
	/// </summary>
    public override void CharacterDamageEnabled(bool enabled)
    {
        Player1.CanTakeDamage = enabled;
        Player2.CanTakeDamage = enabled;
    }
}
