// file:	Assets\UnityTestTools\Assertions\Editor\DropDownControl.cs
//
// summary:	Implements the drop down control class

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   (Serializable) a drop down control. </summary>
    ///
 
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>

    [Serializable]
    internal class DropDownControl<T>
    {
        /// <summary>   Options for controlling the button layout. </summary>
        private readonly GUILayoutOption[] m_ButtonLayoutOptions = { GUILayout.ExpandWidth(true) };
        /// <summary>   The = to process. </summary>
        public Func<T, string> convertForButtonLabel = s => s.ToString();
        /// <summary>   The = to process. </summary>
        public Func<T, string> convertForGUIContent = s => s.ToString();
        /// <summary>   The = to process. </summary>
        public Func<T[], bool> ignoreConvertForGUIContent = t => t.Length <= 40;
        /// <summary>   The print context menu. </summary>
        public Action<T> printContextMenu = null;
        /// <summary>   The tooltip. </summary>
        public string tooltip = "";

        /// <summary>   The selected value. </summary>
        private object m_SelectedValue;

        /// <summary>   Draws. </summary>
        ///
     
        ///
        /// <param name="selected">         The selected. </param>
        /// <param name="options">          Options for controlling the operation. </param>
        /// <param name="onValueSelected">  The on value selected. </param>

        public void Draw(T selected, T[] options, Action<T> onValueSelected)
        {
            Draw(null,
                 selected,
                 options,
                 onValueSelected);
        }

        /// <summary>   Draws. </summary>
        ///
     
        ///
        /// <param name="label">            The label. </param>
        /// <param name="selected">         The selected. </param>
        /// <param name="options">          Options for controlling the operation. </param>
        /// <param name="onValueSelected">  The on value selected. </param>

        public void Draw(string label, T selected, T[] options, Action<T> onValueSelected)
        {
            Draw(label, selected, () => options, onValueSelected);
        }

        /// <summary>   Draws. </summary>
        ///
     
        ///
        /// <param name="label">            The label. </param>
        /// <param name="selected">         The selected. </param>
        /// <param name="loadOptions">      Options for controlling the load. </param>
        /// <param name="onValueSelected">  The on value selected. </param>

        public void Draw(string label, T selected, Func<T[]> loadOptions, Action<T> onValueSelected)
        {
            if (!string.IsNullOrEmpty(label))
                EditorGUILayout.BeginHorizontal();
            var guiContent = new GUIContent(label);
            var labelSize = EditorStyles.label.CalcSize(guiContent);

            if (!string.IsNullOrEmpty(label))
                GUILayout.Label(label, EditorStyles.label, GUILayout.Width(labelSize.x));

            if (GUILayout.Button(new GUIContent(convertForButtonLabel(selected), tooltip),
                                 EditorStyles.popup, m_ButtonLayoutOptions))
            {
                if (Event.current.button == 0)
                {
                    PrintMenu(loadOptions());
                }
                else if (printContextMenu != null && Event.current.button == 1)
                    printContextMenu(selected);
            }

            if (m_SelectedValue != null)
            {
                onValueSelected((T)m_SelectedValue);
                m_SelectedValue = null;
            }
            if (!string.IsNullOrEmpty(label))
                EditorGUILayout.EndHorizontal();
        }

        /// <summary>   Print menu. </summary>
        ///
     
        ///
        /// <param name="options">  Options for controlling the operation. </param>

        public void PrintMenu(T[] options)
        {
            var menu = new GenericMenu();
            foreach (var s in options)
            {
                var localS = s;
                menu.AddItem(new GUIContent((ignoreConvertForGUIContent(options) ? localS.ToString() : convertForGUIContent(localS))),
                             false,
                             () => { m_SelectedValue = localS; }
                             );
            }
            menu.ShowAsContext();
        }
    }
}
