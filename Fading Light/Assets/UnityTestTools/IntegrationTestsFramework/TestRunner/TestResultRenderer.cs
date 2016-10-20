// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\TestResultRenderer.cs
//
// summary:	Implements the test result renderer class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>   A test result renderer. </summary>
///
/// <remarks>    . </remarks>

public class TestResultRenderer
{
    /// <summary>   A styles. </summary>
    ///
 

    private static class Styles
    {
        /// <summary>   The succeed label style. </summary>
        public static readonly GUIStyle SucceedLabelStyle;
        /// <summary>   The failed label style. </summary>
        public static readonly GUIStyle FailedLabelStyle;
        /// <summary>   The failed messages style. </summary>
        public static readonly GUIStyle FailedMessagesStyle;

        /// <summary>   Static constructor. </summary>
        ///
     

        static Styles()
        {
            SucceedLabelStyle = new GUIStyle("label");
            SucceedLabelStyle.normal.textColor = Color.green;
            SucceedLabelStyle.fontSize = 48;

            FailedLabelStyle = new GUIStyle("label");
            FailedLabelStyle.normal.textColor = Color.red;
            FailedLabelStyle.fontSize = 32;

            FailedMessagesStyle = new GUIStyle("label");
            FailedMessagesStyle.wordWrap = false;
            FailedMessagesStyle.richText = true;
        }
    }
    /// <summary>   Collection of tests. </summary>
    private readonly Dictionary<string, List<ITestResult>> m_TestCollection = new Dictionary<string, List<ITestResult>>();

    /// <summary>   True to show, false to hide the results. </summary>
    private bool m_ShowResults;
    /// <summary>   The scroll position. </summary>
    Vector2 m_ScrollPosition;
    /// <summary>   Number of failures. </summary>
    private int m_FailureCount;

    /// <summary>   Shows the results. </summary>
    ///
 

    public void ShowResults()
    {
        m_ShowResults = true;
        Cursor.visible = true;
    }

    /// <summary>   Adds the results to 'result'. </summary>
    ///
 
    ///
    /// <param name="sceneName">    Name of the scene. </param>
    /// <param name="result">       The result. </param>

    public void AddResults(string sceneName, ITestResult result)
    {
        if (!m_TestCollection.ContainsKey(sceneName))
            m_TestCollection.Add(sceneName, new List<ITestResult>());
        m_TestCollection[sceneName].Add(result);
        if (result.Executed && !result.IsSuccess)
            m_FailureCount++;
    }

    /// <summary>   Draws this object. </summary>
    ///
 

    public void Draw()
    {
        if (!m_ShowResults) return;
        if (m_TestCollection.Count == 0)
        {
            GUILayout.Label("All test succeeded", Styles.SucceedLabelStyle, GUILayout.Width(600));
        }
        else
        {
            int count = m_TestCollection.Sum (testGroup => testGroup.Value.Count);
            GUILayout.Label(count + " tests failed!", Styles.FailedLabelStyle);

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, GUILayout.ExpandWidth(true));
            var text = "";
            foreach (var testGroup in m_TestCollection)
            {
                text += "<b><size=18>" + testGroup.Key + "</size></b>\n";
                text += string.Join("\n", testGroup.Value
                                    .Where(result => !result.IsSuccess)
                                    .Select(result => result.Name + " " + result.ResultState + "\n" + result.Message)
                                    .ToArray());
            }
            GUILayout.TextArea(text, Styles.FailedMessagesStyle);
            GUILayout.EndScrollView();
        }
        if (GUILayout.Button("Close"))
            Application.Quit();
    }

    /// <summary>   Failure count. </summary>
    ///
 
    ///
    /// <returns>   An int. </returns>

    public int FailureCount()
    {
        return m_FailureCount;
    }
}
