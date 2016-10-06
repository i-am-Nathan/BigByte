using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Storyline : MonoBehaviour {

    public PlayerController Player1;
    public Player2Controller Player2;
    public List<MoleManContoller> MoleMen;
    public TorchFuelController TorchController;
    public List<GameObject> ReferencePoints;

    private int _currentStep = 0;
    private bool _done = false;
    private ToolTips _tips;
    private bool _tipsDone = false;
    private float _startDisplay;
    // Use this for initialization
    void Start () {
        _tips = new ToolTips();
        _tips.DisableToolTips();
      
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time - _startDisplay > 5 && !_tipsDone)
        {
            _tips.DisableToolTips();
        }
        if (_done)
        {
            return;
        }

        if(_currentStep == 0)
        {
            TorchController.IsDisabled = true;
            //Moleman walking to players
            _done = true;
            Player1.IsDisabled = true;
            Player2.IsDisabled = true;
            MoleMen[0].IsDisabled = false;
        }else if(_currentStep == 1)
        {
            //Moleman walks out
            _done = true;
            MoleMen[0].Next();
            TorchController.SwapPlayers();
            MoleMen[0].transform.Find("Spotlight").gameObject.SetActive(false);
            MoleMen[0].transform.Find("hips/Torch Light Holder").gameObject.SetActive(false);
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;
            MoleMen[0].IsDisabled = false;

            //Turn on keys for X seconds
            _tips.EnableToolTips();
            _startDisplay = Time.time;

        }
        else if (_currentStep == 2)
        {
            //Moleman walks out
            _done = true;
            MoleMen[0].IsDisabled = false;
            Player1.IsDisabled = false;
            Player2.IsDisabled = false;


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

        else if (_currentStep == 1)
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

    public void DisableMoleMan()
    {
        MoleMen[0].IsDisabled = true;
    }

    public void EnableMoleMan()
    {
        MoleMen[0].IsDisabled = false;
    }
}
