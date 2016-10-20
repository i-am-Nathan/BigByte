// file:	Assets\Scripts\GameControl\Storyline_Level2.cs
//
// summary:	Implements the storyline level 2 class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

<<<<<<< HEAD
/// <summary>   A storyline level 2. </summary>
///
/// <remarks>    . </remarks>

public class Storyline_Level2 : Storyline
{


    /// <summary>   The first player. </summary>
=======
/// <summary>
/// Storyline class for the second level
/// </summary>
public class Storyline_Level2 : Storyline
{
	// Cutscenes and other game objects to be used as part of the storyline
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
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

<<<<<<< HEAD
    /// <summary>   Dialogues the complete. </summary>
    ///
 

=======
	/// <summary>
	/// Called when dialogue is complete
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
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

<<<<<<< HEAD
    /// <summary>   Enables the mole man. </summary>
    ///
 

=======
	/// <summary>
	/// Activating the moleman
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
    public override void EnableMoleMan()
    {
        //throw new NotImplementedException();
    }

<<<<<<< HEAD
    /// <summary>   Nexts this instance. </summary>
    ///
 

=======
	/// <summary>
	/// Moving to the next lines in the dialogue
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
    public override void Next()
    {
        if(_currentStep == 4)
        {
            _currentStep++;
            _done = false;
        }
       
    }

<<<<<<< HEAD
    /// <summary>   Deletes the first mole man from list. </summary>
    ///
 

=======
	/// <summary>
	/// Activating the next moleman in the scene
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
    public override void NextMoleMan()
    {
        if (_currentStep == 5)
        {
            _currentStep++;
            _done = false;
        }
        //throw new NotImplementedException();
    }

<<<<<<< HEAD
    /// <summary>   Disables the mole man. </summary>
    ///
 

=======
	/// <summary>
	/// Called near the start of the level
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
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
<<<<<<< HEAD

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start () {
	
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

=======
	
	/// <summary>
	/// Used to make the moleman walk and also to enable cutscenes based on the current steps
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
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

<<<<<<< HEAD
    /// <summary>   Character damage enabled. </summary>
    ///
 
    ///
    /// <param name="enabled">  True to enable, false to disable. </param>

=======
	/// <summary>
	/// Enabling the character damage (as it is disabled during cutscenes)
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
    public override void CharacterDamageEnabled(bool enabled)
    {
        Player1.CanTakeDamage = enabled;
        Player2.CanTakeDamage = enabled;
    }
}
