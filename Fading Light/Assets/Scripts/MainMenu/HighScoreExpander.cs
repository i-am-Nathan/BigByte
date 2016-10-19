using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreExpander : MonoBehaviour {

    private bool _expanded = false;
	// Use this for initialization 
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void ToggleExpand()
    {
        var textBox = transform.Find("Description").gameObject.GetComponent<Text>();

        var layoutElement = GetComponent<LayoutElement>();

        var buttonTextBox = transform.Find("Button/Text").GetComponent<Text>();

        _expanded = !_expanded;

        if (_expanded)
        {
            textBox.text = GetComponent<HighScorePanel>().Score.ExpandedString();
            layoutElement.preferredHeight = 160;
            buttonTextBox.text = "Less";
        }
        else
        {
            textBox.text = GetComponent<HighScorePanel>().Score.TimeString();
            layoutElement.preferredHeight = 60;
            buttonTextBox.text = "More";
        }

        
    }
}
