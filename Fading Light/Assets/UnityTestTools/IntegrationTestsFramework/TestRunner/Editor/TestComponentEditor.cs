// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\TestComponentEditor.cs
//
// summary:	Implements the test component editor class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor.SceneManagement;

namespace UnityTest
{
    /// <summary>   Editor for test component. </summary>
    ///
 

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TestComponent))]
    public class TestComponentEditor : Editor
    {
        /// <summary>   The expect exception. </summary>
        private SerializedProperty m_ExpectException;
        /// <summary>   List of expected exceptions. </summary>
        private SerializedProperty m_ExpectedExceptionList;
        /// <summary>   The ignored. </summary>
        private SerializedProperty m_Ignored;
        /// <summary>   The succeed assertions. </summary>
        private SerializedProperty m_SucceedAssertions;
        /// <summary>   The succeed when exception is thrown. </summary>
        private SerializedProperty m_SucceedWhenExceptionIsThrown;
        /// <summary>   The timeout. </summary>
        private SerializedProperty m_Timeout;

        #region GUI Contens

        /// <summary>   The graphical user interface expect exception. </summary>
        private readonly GUIContent m_GUIExpectException = new GUIContent("Expect exception", "Should the test expect an exception");
        /// <summary>   List of graphical user interface expect exceptions. </summary>
        private readonly GUIContent m_GUIExpectExceptionList = new GUIContent("Expected exception list", "A comma separated list of exception types which will not fail the test when thrown");
        /// <summary>   The graphical user interface ignore. </summary>
        private readonly GUIContent m_GUIIgnore = new GUIContent("Ignore", "Ignore the tests in runs");
        /// <summary>   The graphical user interface include platforms. </summary>
        private readonly GUIContent m_GUIIncludePlatforms = new GUIContent("Included platforms", "Platform on which the test should run");
        /// <summary>   The graphical user interface succed on assertions. </summary>
        private readonly GUIContent m_GUISuccedOnAssertions = new GUIContent("Succeed on assertions", "Succeed after all assertions are executed");
        /// <summary>   The graphical user interface succeed when exception is thrown. </summary>
        private readonly GUIContent m_GUISucceedWhenExceptionIsThrown = new GUIContent("Succeed when exception is thrown", "Should the test succeed when an expected exception is thrown");
        /// <summary>   Name of the graphical user interface test. </summary>
        private readonly GUIContent m_GUITestName = new GUIContent("Test name", "Name of the test (is equal to the GameObject name)");
        /// <summary>   The graphical user interface timeout. </summary>
        private readonly GUIContent m_GUITimeout = new GUIContent("Timeout", "Number of seconds after which the test will timeout");

        #endregion

        /// <summary>   Executes the enable action. </summary>
        ///
     

        public void OnEnable()
        {
            m_Timeout = serializedObject.FindProperty("timeout");
            m_Ignored = serializedObject.FindProperty("ignored");
            m_SucceedAssertions = serializedObject.FindProperty("succeedAfterAllAssertionsAreExecuted");
            m_ExpectException = serializedObject.FindProperty("expectException");
            m_ExpectedExceptionList = serializedObject.FindProperty("expectedExceptionList");
            m_SucceedWhenExceptionIsThrown = serializedObject.FindProperty("succeedWhenExceptionIsThrown");
        }

        /// <summary>
        /// <para>
        /// Implement this function to make a custom inspector.</para>
        /// </summary>
        ///
     

        public override void OnInspectorGUI()
        {
            var component = (TestComponent)target;

            if (component.dynamic)
            {
				if(GUILayout.Button("Reload dynamic tests"))
				{
					TestComponent.DestroyAllDynamicTests();
	                Selection.objects = new Object[0];
	                IntegrationTestsRunnerWindow.selectedInHierarchy = false;
	                GUIUtility.ExitGUI();
	                return;
				}
				EditorGUILayout.HelpBox("This is a test generated from code. No changes in the component will be persisted.", MessageType.Info);
            }

            if (component.IsTestGroup())
            {
                EditorGUI.BeginChangeCheck();
                var newGroupName = EditorGUILayout.TextField(m_GUITestName, component.name);
                if (EditorGUI.EndChangeCheck()) component.name = newGroupName;

                serializedObject.ApplyModifiedProperties();
                return;
            }

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

            EditorGUI.BeginChangeCheck();
            var newName = EditorGUILayout.TextField(m_GUITestName, component.name);
            if (EditorGUI.EndChangeCheck()) component.name = newName;

            if (component.platformsToIgnore == null)
            {
                component.platformsToIgnore = GetListOfIgnoredPlatforms(Enum.GetNames(typeof(TestComponent.IncludedPlatforms)), (int)component.includedPlatforms);
            }

            var enumList = Enum.GetNames(typeof(RuntimePlatform));
            var flags = GetFlagList(enumList, component.platformsToIgnore);
            flags = EditorGUILayout.MaskField(m_GUIIncludePlatforms, flags, enumList, EditorStyles.popup);
            var newList = GetListOfIgnoredPlatforms(enumList, flags);
            if (!component.dynamic)
                component.platformsToIgnore = newList;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(m_Timeout, m_GUITimeout);
            EditorGUILayout.PropertyField(m_Ignored, m_GUIIgnore);
            EditorGUILayout.PropertyField(m_SucceedAssertions, m_GUISuccedOnAssertions);
            EditorGUILayout.PropertyField(m_ExpectException, m_GUIExpectException);

            EditorGUI.BeginDisabledGroup(!m_ExpectException.boolValue);
            EditorGUILayout.PropertyField(m_ExpectedExceptionList, m_GUIExpectExceptionList);
            EditorGUILayout.PropertyField(m_SucceedWhenExceptionIsThrown, m_GUISucceedWhenExceptionIsThrown);
            EditorGUI.EndDisabledGroup();

            if (!component.dynamic)
                serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        /// <summary>   Gets list of ignored platforms. </summary>
        ///
     
        ///
        /// <param name="enumList"> List of enums. </param>
        /// <param name="flags">    The flags. </param>
        ///
        /// <returns>   An array of string. </returns>

        private string[] GetListOfIgnoredPlatforms(string[] enumList, int flags)
        {
            var notSelectedPlatforms = new List<string>();
            for (int i = 0; i < enumList.Length; i++)
            {
                var sel = (flags & (1 << i)) != 0;
                if (!sel) notSelectedPlatforms.Add(enumList[i]);
            }
            return notSelectedPlatforms.ToArray();
        }

        /// <summary>   Gets flag list. </summary>
        ///
     
        ///
        /// <param name="enumList">             List of enums. </param>
        /// <param name="platformsToIgnore">    The platforms to ignore. </param>
        ///
        /// <returns>   The flag list. </returns>

        private int GetFlagList(string[] enumList, string[] platformsToIgnore)
        {
            int flags = ~0;
            for (int i = 0; i < enumList.Length; i++)
                if (platformsToIgnore != null && platformsToIgnore.Any(s => s == enumList[i]))
                    flags &= ~(1 << i);
            return flags;
        }
    }
}
