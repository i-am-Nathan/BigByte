// file:	Assets\UnityTestTools\Common\Editor\TestFilterSettings.cs
//
// summary:	Implements the test filter settings class

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace UnityTest
{
    /// <summary>   A test filter settings. </summary>
    ///
 

    public class TestFilterSettings
    {
        /// <summary>   True to show, false to hide the succeeded. </summary>
        public bool ShowSucceeded;
        /// <summary>   True to show, false to hide the failed. </summary>
        public bool ShowFailed;
        /// <summary>   True to show, false to hide the ignored. </summary>
        public bool ShowIgnored;
        /// <summary>   True to show, false to hide the not run. </summary>
        public bool ShowNotRun;
        
        /// <summary>   Name of the filter by. </summary>
        public string FilterByName;
        /// <summary>   Category the filter by belongs to. </summary>
        public int FilterByCategory;
        
        /// <summary>   The succeeded button. </summary>
        private GUIContent _succeededBtn;
        /// <summary>   The failed button. </summary>
        private GUIContent _failedBtn;
        /// <summary>   The ignored button. </summary>
        private GUIContent _ignoredBtn;
        /// <summary>   The not run button. </summary>
        private GUIContent _notRunBtn;
        
        /// <summary>   Categories the available belongs to. </summary>
        public string[] AvailableCategories;
        
        /// <summary>   The preferences key. </summary>
        private readonly string _prefsKey;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="prefsKey"> The preferences key. </param>

        public TestFilterSettings(string prefsKey)
        {
            _prefsKey = prefsKey;
            Load();
            UpdateCounters(Enumerable.Empty<ITestResult>());
        }

        /// <summary>   Loads this object. </summary>
        ///
     

        public void Load()
        {
            ShowSucceeded = EditorPrefs.GetBool(_prefsKey + ".ShowSucceeded", true);
            ShowFailed = EditorPrefs.GetBool(_prefsKey + ".ShowFailed", true);
            ShowIgnored = EditorPrefs.GetBool(_prefsKey + ".ShowIgnored", true);
            ShowNotRun = EditorPrefs.GetBool(_prefsKey + ".ShowNotRun", true);
            FilterByName = EditorPrefs.GetString(_prefsKey + ".FilterByName", string.Empty);
            FilterByCategory = EditorPrefs.GetInt(_prefsKey + ".FilterByCategory", 0);
        }

        /// <summary>   Saves this object. </summary>
        ///
     

        public void Save()
        {
            EditorPrefs.SetBool(_prefsKey + ".ShowSucceeded", ShowSucceeded);
            EditorPrefs.SetBool(_prefsKey + ".ShowFailed", ShowFailed);
            EditorPrefs.SetBool(_prefsKey + ".ShowIgnored", ShowIgnored);
            EditorPrefs.SetBool(_prefsKey + ".ShowNotRun", ShowNotRun);
            EditorPrefs.SetString(_prefsKey + ".FilterByName", FilterByName);
            EditorPrefs.SetInt(_prefsKey + ".FilterByCategory", FilterByCategory);
        }

        /// <summary>   Updates the counters described by results. </summary>
        ///
     
        ///
        /// <param name="results">  The results. </param>

        public void UpdateCounters(IEnumerable<ITestResult> results)
        {
            var summary = new ResultSummarizer(results);
            
            _succeededBtn = new GUIContent(summary.Passed.ToString(), Icons.SuccessImg, "Show tests that succeeded");
            _failedBtn = new GUIContent((summary.Errors + summary.Failures + summary.Inconclusive).ToString(), Icons.FailImg, "Show tests that failed");
            _ignoredBtn = new GUIContent((summary.Ignored + summary.NotRunnable).ToString(), Icons.IgnoreImg, "Show tests that are ignored");
            _notRunBtn = new GUIContent((summary.TestsNotRun - summary.Ignored - summary.NotRunnable).ToString(), Icons.UnknownImg, "Show tests that didn't run");
        }

        /// <summary>   Gets selected categories. </summary>
        ///
     
        ///
        /// <returns>   An array of string. </returns>

        public string[] GetSelectedCategories()
        {
            if(AvailableCategories == null) return new string[0];
            
            return AvailableCategories.Where ((c, i) => (FilterByCategory & (1 << i)) != 0).ToArray();
        }

        /// <summary>   Executes the graphical user interface action. </summary>
        ///
     

        public void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            FilterByName = GUILayout.TextField(FilterByName, "ToolbarSeachTextField", GUILayout.MinWidth(100), GUILayout.MaxWidth(250), GUILayout.ExpandWidth(true));
            if(GUILayout.Button (GUIContent.none, string.IsNullOrEmpty(FilterByName) ? "ToolbarSeachCancelButtonEmpty" : "ToolbarSeachCancelButton"))
                FilterByName = string.Empty;
            
            if (AvailableCategories != null && AvailableCategories.Length > 0)
                FilterByCategory = EditorGUILayout.MaskField(FilterByCategory, AvailableCategories, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(90));
            
            ShowSucceeded = GUILayout.Toggle(ShowSucceeded, _succeededBtn, EditorStyles.toolbarButton);
            ShowFailed = GUILayout.Toggle(ShowFailed, _failedBtn, EditorStyles.toolbarButton);
            ShowIgnored = GUILayout.Toggle(ShowIgnored, _ignoredBtn, EditorStyles.toolbarButton);
            ShowNotRun = GUILayout.Toggle(ShowNotRun, _notRunBtn, EditorStyles.toolbarButton);
            
            if(EditorGUI.EndChangeCheck()) Save ();
        }

        /// <summary>   Builds rendering options. </summary>
        ///
     
        ///
        /// <returns>   The RenderingOptions. </returns>

        public RenderingOptions BuildRenderingOptions()
        {
            var options = new RenderingOptions();
            options.showSucceeded = ShowSucceeded;
            options.showFailed = ShowFailed;
            options.showIgnored = ShowIgnored;
            options.showNotRunned = ShowNotRun;
            options.nameFilter = FilterByName;
            options.categories = GetSelectedCategories();
            return options;
        }
    }
    
}
