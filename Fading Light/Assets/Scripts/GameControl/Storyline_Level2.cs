using UnityEngine;
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
       // throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
