// file:	Assets\Scripts\GameControl\Storyline_Level3.cs
//
// summary:	Implements the storyline level 3 class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

<<<<<<< HEAD
/// <summary>   A storyline level 3. </summary>
///
/// <remarks>    . </remarks>

public class Storyline_Level3 : Storyline
{


    /// <summary>   The first player. </summary>
=======
/// <summary>
/// Storyline class for the third level
/// </summary>
public class Storyline_Level3 : Storyline
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
    /// <summary>   The prisoner. </summary>
    public PrisonerController Prisoner;
    /// <summary>   The boss. </summary>
    public SkeleBoss Boss;
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
    /// <summary>   The block. </summary>
    public GameObject Block;

<<<<<<< HEAD
    /// <summary>   Dialogues the complete. </summary>
    ///
 

=======
	/// <summary>
	/// Dialogues the complete
	/// </summary>
>>>>>>> 9e4d3f99ec3af3f85a42d04e36c84bfd6c4626e8
    public override void DialogueComplete()
    {
        _currentStep++;
        _done = false;
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
        if (_currentStep == 3)
        {
            _done = false;
            _currentStep++;
        }

    }

    /// <summary>   Deletes the first mole man from list. </summary>
    ///
 

    public override void NextMoleMan()
    {
        if (_currentStep == 4 || _currentStep == 2)
        {
            _done = false;
            _currentStep++;
        }
        //throw new NotImplementedException();
    }

    /// <summary>   Disables the mole man. </summary>
    ///
 

    public override void StartText()
    {
        if(_currentStep == 0)
        {
            _currentStep++;
            _done = false;
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

        if(_currentStep == 0)
        {
            TorchController.SwapPlayers();
            _done = true;
        }
        else if(_currentStep == 1)
        {
            CharacterDamageEnabled(false);
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
            CharacterDamageEnabled(true);
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            DestroyObject(Block);
        }else if (_currentStep == 4)
        {
			// Cutscene and camera movement
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
            CharacterDamageEnabled(true);
            TorchController.IsDisabled = false;
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
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
