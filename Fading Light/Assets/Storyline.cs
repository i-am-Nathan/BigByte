using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Storyline : MonoBehaviour {

    public PlayerController Player1;
    public Player2Controller Player2;
    public List<MoleManContoller> MoleMen;
    public List<GameObject> ReferencePoints;

    private int _currentStep = 0;
    private bool _done = false;
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
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            //MoleMan.IsDisabled = true;
        }else if(_currentStep == 1)
        {
            _done = true;
            MoleMen[0].Next();
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;
            MoleMen[0].IsDisabled = false;
        }
	}

    public void Next()
    {

    }

    public void DialogueComplete()
    {
        if(_currentStep == 0)
        {
            _currentStep++;
            _done = false;
        }
    }

    public void MoleManInPosition()
    {
        if(_currentStep == 1)
        {
            MoleMen.RemoveAt(0);
            _currentStep++;
        }
    }


}
