// file:	Assets\UnityTestTools\Common\Editor\Styles.cs
//
// summary:	Implements the styles class

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A styles. </summary>
    ///
 

    public static class Styles
    {
        /// <summary>   The information. </summary>
        public static GUIStyle info;
        /// <summary>   List of tests. </summary>
        public static GUIStyle testList;

        /// <summary>   The selected foldout. </summary>
        public static GUIStyle selectedFoldout;
        /// <summary>   The foldout. </summary>
        public static GUIStyle foldout;
        /// <summary>   The toolbar label. </summary>
        public static GUIStyle toolbarLabel;
        
        /// <summary>   Name of the test. </summary>
        public static GUIStyle testName;

        /// <summary>   The selected color. </summary>
        private static readonly Color k_SelectedColor = new Color(0.3f, 0.5f, 0.85f);

        /// <summary>   Static constructor. </summary>
        ///
     

        static Styles()
        {
            info = new GUIStyle(EditorStyles.wordWrappedLabel);
            info.wordWrap = false;
            info.stretchHeight = true;
            info.margin.right = 15;

            testList = new GUIStyle("CN Box");
            testList.margin.top = 0;
            testList.padding.left = 3;

            foldout = new GUIStyle(EditorStyles.foldout);
            selectedFoldout = new GUIStyle(EditorStyles.foldout);
            selectedFoldout.onFocused.textColor = selectedFoldout.focused.textColor =
                                                      selectedFoldout.onActive.textColor = selectedFoldout.active.textColor =
                                                                                               selectedFoldout.onNormal.textColor = selectedFoldout.normal.textColor = k_SelectedColor;
                                                                                               
            toolbarLabel = new GUIStyle(EditorStyles.toolbarButton);
            toolbarLabel.normal.background = null;
            toolbarLabel.contentOffset = new Vector2(0, -2);
            
            testName = new GUIStyle(EditorStyles.label);
            testName.padding.left += 12;
            testName.focused.textColor = testName.onFocused.textColor = k_SelectedColor;
        }
    }
}
