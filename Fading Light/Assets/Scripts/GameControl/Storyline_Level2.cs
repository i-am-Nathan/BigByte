// file:	Assets\Scripts\GameControl\Storyline_Level2.cs
//
// summary:	Implements the storyline level 2 class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>   A storyline level 2. </summary>
///
/// <remarks>    . </remarks>

public class Storyline_Level2 : Storyline
{


    /// <summary>   The first player. </summary>
    public PlayerController Player1;
    /// <summary>   The second player. </summary>
    public Player2Controller Player2;
    /// <summary>   The mole men. </summary>
    public List<MoleManContoller> MoleMen;
    /// <summary>   The torch controller. </summary>
    public TorchFuelController TorchController;
    /// <summary>   The reference points. </summary>
    public List<GameObject> ReferencePoints;
    /// <summary>   The camera rig. </summary>
    public GameObject CameraRig;
    /// <summary>   The boss. </summary>
    public MoleDoggy Boss;
    /// <summary>   The cut scene positions. </summary>
    public List<GameObject> CutScenePositions;
    /// <summary>   The customer scene targets. </summary>
    public List<GameObject> CustSceneTargets;

    /// <summary>   The current step. </summary>
    public int _currentStep = 0;
    /// <summary>   True to done. </summary>
    private bool _done = false;
    /// <summary>   The tips. </summary>
    private ToolTips _tips;
    /// <summary>   True to tips done. </summary>
    private bool _tipsDone = false;
    /// <summary>   The start display. </summary>
    private float _startDisplay;

    /// <summary>   Dialogues the complete. </summary>
    ///
 

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

    /// <summary>   Enables the mole man. </summary>
    ///
 

    public override void EnableMoleMan()
    {
        //throw new NotImplementedException();
    }

    /// <summary>   Nexts this instance. </summary>
    ///
 

    public override void Next()
    {
        if(_currentStep == 4)
        {
            _currentStep++;
            _done = false;
        }
       
    }

    /// <summary>   Deletes the first mole man from list. </summary>
    ///
 

    public override void NextMoleMan()
    {
        if (_currentStep == 5)
        {
            _currentStep++;
            _done = false;
        }
        //throw new NotImplementedException();
    }

    /// <summary>   Disables the mole man. </summary>
    ///
 

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

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start () {
	
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

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
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[0];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[0];
            TorchController.IsDisabled = true;
            //Moleman walking to players
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            //MoleMen[0].IsDisabled = false;
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
            TorchController.IsDisabled = false;
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            CharacterDamageEnabled(true);
            _done = true;
        }
    }

    /// <summary>   Character damage enabled. </summary>
    ///
 
    ///
    /// <param name="enabled">  True to enable, false to disable. </param>

    public override void CharacterDamageEnabled(bool enabled)
    {
        Player1.CanTakeDamage = enabled;
        Player2.CanTakeDamage = enabled;
    }
}
