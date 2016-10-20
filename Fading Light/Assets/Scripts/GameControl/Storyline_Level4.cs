// file:	Assets\Scripts\GameControl\Storyline_Level4.cs
//
// summary:	Implements the storyline level 4 class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>   A storyline level 4. </summary>
///
/// <remarks>    . </remarks>

public class Storyline_Level4 : Storyline
{
<<<<<<< HEAD


    /// <summary>   The first player. </summary>
=======
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
    /// <summary>   Manager for small mole. </summary>
    public MoleManContoller SmallMoleMan;
    /// <summary>   The cut scene positions. </summary>
    public List<GameObject> CutScenePositions;
    /// <summary>   The customer scene targets. </summary>
    public List<GameObject> CustSceneTargets;

    /// <summary>   The boss. </summary>
    public MolemanBoss Boss;

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
    /// <summary>   True if finished. </summary>
    private bool _finished = false;

    /// <summary>   Dialogues the complete. </summary>
    ///
 

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
        if(_currentStep == 0)
        {
            _currentStep++;
            _done = false;
        }
       
    }

    /// <summary>   Deletes the first mole man from list. </summary>
    ///
 

    public override void NextMoleMan()
    {
        if (_currentStep == 1)
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
    }

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start () {
	
	}

    // Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        if (_done)
        {
            
            return;
        }

        _finished = false;

        if (_currentStep == 0)
        {
            SmallMoleMan.IsDisabled = true;
            TorchController.SwapPlayers();
            //zoom in on prisoner
            CameraRig.GetComponent<PlayerCam>().SwoopPositionTarget = CutScenePositions[0];
            CameraRig.GetComponent<PlayerCam>().SwoopAngleTarget = CustSceneTargets[0];
            CameraRig.GetComponent<PlayerCam>().CameraState = 1;
            TorchController.IsDisabled = true;
            //Moleman walking to players
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            _done = true;
           
            
        }
        else if (_currentStep == 1)
        {

            Boss.gameObject.SetActive(true);
            MoleManContoller moleman = GameObject.FindGameObjectWithTag("Moleman").GetComponent<MoleManContoller>();
            GameObject.FindGameObjectWithTag("Mud").gameObject.SetActive(true);
            moleman.Sink();
            //Talking done
            Boss.BeginCutscene(this);
            Boss.gameObject.GetComponent<Animation>().Play();
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;
            _done = true;
           
        }
        else if (_currentStep == 2)
        {
            //Transformation done
            CameraRig.GetComponent<PlayerCam>().CameraState = 0;
            _done = true;
            _finished = true;
        }
    }

    /// <summary>   Query if this object is done. </summary>
    ///
 
    ///
    /// <returns>   True if done, false if not. </returns>

    public bool IsDone()
    {
        return _finished;
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
