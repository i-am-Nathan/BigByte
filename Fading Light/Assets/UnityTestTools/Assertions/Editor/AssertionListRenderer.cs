// file:	Assets\UnityTestTools\Assertions\Editor\AssertionListRenderer.cs
//
// summary:	Implements the assertion list renderer class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   Interface for list renderer. </summary>
    ///
 

    public interface IListRenderer
    {
        /// <summary>   Renders this object. </summary>
        ///
        /// <param name="allAssertions">    all assertions. </param>
        /// <param name="foldMarkers">      The fold markers. </param>

        void Render(IEnumerable<AssertionComponent> allAssertions, List<string> foldMarkers);
    }

    /// <summary>   An assertion list renderer. </summary>
    ///
 
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>

    public abstract class AssertionListRenderer<T> : IListRenderer
    {
        /// <summary>   A styles. </summary>
        ///
     

        private static class Styles
        {
            /// <summary>   The red label. </summary>
            public static readonly GUIStyle redLabel;

            /// <summary>   Static constructor. </summary>
            ///
         

            static Styles()
            {
                redLabel = new GUIStyle(EditorStyles.label);
                redLabel.normal.textColor = Color.red;
            }
        }

        /// <summary>   Renders this object. </summary>
        ///
     
        ///
        /// <param name="allAssertions">    all assertions. </param>
        /// <param name="foldMarkers">      The fold markers. </param>

        public void Render(IEnumerable<AssertionComponent> allAssertions, List<string> foldMarkers)
        {
            foreach (var grouping in GroupResult(allAssertions))
            {
                var key = GetStringKey(grouping.Key);
                bool isFolded = foldMarkers.Contains(key);
                if (key != "")
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUI.BeginChangeCheck();
                    isFolded = PrintFoldout(isFolded,
                                            grouping.Key);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (isFolded)
                            foldMarkers.Add(key);
                        else
                            foldMarkers.Remove(key);
                    }
                    EditorGUILayout.EndHorizontal();
                    if (isFolded)
                        continue;
                }
                foreach (var assertionComponent in grouping)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    if (key != "")
                        GUILayout.Space(15);

                    var assertionKey = assertionComponent.GetHashCode().ToString();
                    bool isDetailsFolded = foldMarkers.Contains(assertionKey);

                    EditorGUI.BeginChangeCheck();
                    if (GUILayout.Button("",
                                         EditorStyles.foldout,
                                         GUILayout.Width(15)))
                    {
                        isDetailsFolded = !isDetailsFolded;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (isDetailsFolded)
                            foldMarkers.Add(assertionKey);
                        else
                            foldMarkers.Remove(assertionKey);
                    }
                    PrintFoldedAssertionLine(assertionComponent);
                    EditorGUILayout.EndHorizontal();

                    if (isDetailsFolded)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (key != "")
                            GUILayout.Space(15);
                        PrintAssertionLineDetails(assertionComponent);
                        EditorGUILayout.EndHorizontal();
                    }
                    GUILayout.Box("", new[] {GUILayout.ExpandWidth(true), GUILayout.Height(1)});

                    EditorGUILayout.EndVertical();
                }
            }
        }

        /// <summary>   Enumerates group result in this collection. </summary>
        ///
     
        ///
        /// <param name="assertionComponents">  The assertion components. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process group result in this collection.
        /// </returns>

        protected abstract IEnumerable<IGrouping<T, AssertionComponent>> GroupResult(IEnumerable<AssertionComponent> assertionComponents);

        /// <summary>   Gets string key. </summary>
        ///
     
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The string key. </returns>

        protected virtual string GetStringKey(T key)
        {
            return key.GetHashCode().ToString();
        }

        /// <summary>   Print foldout. </summary>
        ///
     
        ///
        /// <param name="isFolded"> True if this object is folded. </param>
        /// <param name="key">      The key. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected virtual bool PrintFoldout(bool isFolded, T key)
        {
            var content = new GUIContent(GetFoldoutDisplayName(key));
            var size = EditorStyles.foldout.CalcSize(content);

            var rect = GUILayoutUtility.GetRect(content,
                                                EditorStyles.foldout,
                                                GUILayout.MaxWidth(size.x));
            var res = EditorGUI.Foldout(rect,
                                        !isFolded,
                                        content,
                                        true);

            return !res;
        }

        /// <summary>   Gets foldout display name. </summary>
        ///
     
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The foldout display name. </returns>

        protected virtual string GetFoldoutDisplayName(T key)
        {
            return key.ToString();
        }

        /// <summary>   Print folded assertion line. </summary>
        ///
     
        ///
        /// <param name="assertionComponent">   The assertion component. </param>

        protected virtual void PrintFoldedAssertionLine(AssertionComponent assertionComponent)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
            PrintPath(assertionComponent.Action.go,
                      assertionComponent.Action.thisPropertyPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(250));
            var labelStr = assertionComponent.Action.GetType().Name;
            var labelStr2 = assertionComponent.Action.GetConfigurationDescription();
            if (labelStr2 != "")
                labelStr += "( " + labelStr2 + ")";
            EditorGUILayout.LabelField(labelStr);
            EditorGUILayout.EndVertical();

            if (assertionComponent.Action is ComparerBase)
            {
                var comparer = assertionComponent.Action as ComparerBase;

                var otherStrVal = "(no value selected)";
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
                switch (comparer.compareToType)
                {
                    case ComparerBase.CompareToType.CompareToObject:
                        if (comparer.other != null)
                        {
                            PrintPath(comparer.other,
                                      comparer.otherPropertyPath);
                        }
                        else
                        {
                            EditorGUILayout.LabelField(otherStrVal,
                                                       Styles.redLabel);
                        }
                        break;
                    case ComparerBase.CompareToType.CompareToConstantValue:
                        otherStrVal = comparer.ConstValue.ToString();
                        EditorGUILayout.LabelField(otherStrVal);
                        break;
                    case ComparerBase.CompareToType.CompareToNull:
                        otherStrVal = "null";
                        EditorGUILayout.LabelField(otherStrVal);
                        break;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.LabelField("");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        /// <summary>   Print assertion line details. </summary>
        ///
     
        ///
        /// <param name="assertionComponent">   The assertion component. </param>

        protected virtual void PrintAssertionLineDetails(AssertionComponent assertionComponent)
        {
            EditorGUILayout.BeginHorizontal();


            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(320));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Attached to",
                                       GUILayout.Width(70));
            var sss = EditorStyles.objectField.CalcSize(new GUIContent(assertionComponent.gameObject.name));
            EditorGUILayout.ObjectField(assertionComponent.gameObject,
                                        typeof(GameObject),
                                        true,
                                        GUILayout.Width(sss.x));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(250));
            EditorGUILayout.EnumMaskField(assertionComponent.checkMethods,
                                          EditorStyles.popup,
                                          GUILayout.MaxWidth(150));
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Disabled",
                                       GUILayout.Width(55));
            assertionComponent.enabled = !EditorGUILayout.Toggle(!assertionComponent.enabled,
                                                                 GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>   Print path. </summary>
        ///
     
        ///
        /// <param name="go">           The go. </param>
        /// <param name="propertyPath"> Full pathname of the property file. </param>

        private void PrintPath(GameObject go, string propertyPath)
        {
            string contentString = "";
            GUIStyle styleThisPath = EditorStyles.label;
            if (go != null)
            {
                var sss = EditorStyles.objectField.CalcSize(new GUIContent(go.name));
                EditorGUILayout.ObjectField(
                    go,
                    typeof(GameObject),
                    true,
                    GUILayout.Width(sss.x));

                if (!string.IsNullOrEmpty(propertyPath))
                    contentString = "." + propertyPath;
            }
            else
            {
                contentString = "(no value selected)";
                styleThisPath = Styles.redLabel;
            }

            var content = new GUIContent(contentString,
                                         contentString);
            var rect = GUILayoutUtility.GetRect(content,
                                                EditorStyles.label,
                                                GUILayout.MaxWidth(200));

            EditorGUI.LabelField(rect,
                                 content,
                                 styleThisPath);
        }
    }
}
