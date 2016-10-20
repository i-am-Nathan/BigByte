using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Storyline_Level4 : Storyline
{


    public PlayerController Player1;
    public Player2Controller Player2;
    public List<MoleManContoller> MoleMen;
    public TorchFuelController TorchController;
    public List<GameObject> ReferencePoints;
    public GameObject CameraRig;
    public MoleManContoller SmallMoleMan;
    public List<GameObject> CutScenePositions;
    public List<GameObject> CustSceneTargets;

    public MolemanBoss Boss;

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
        if(_currentStep == 0)
        {
            _currentStep++;
            _done = false;
        }
       
    }

    public override void NextMoleMan()
    {
        if (_currentStep == 1)
        {
            _currentStep++;
            _done = false;
        }
        //throw new NotImplementedException();
    }

    public override void StartText()
    {
    }

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (_done)
        {
            
            return;
        }

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
            Debug.Log("TALKING DONE G");
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
        }
    }

    public override void CharacterDamageEnabled(bool enabled)
    {
        Player1.CanTakeDamage = enabled;
        Player2.CanTakeDamage = enabled;
    }
}
