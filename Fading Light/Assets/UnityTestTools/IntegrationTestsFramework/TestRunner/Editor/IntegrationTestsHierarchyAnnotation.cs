// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\IntegrationTestsHierarchyAnnotation.cs
//
// summary:	Implements the integration tests hierarchy annotation class

using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityTest
{
    /// <summary>   An integration tests hierarchy annotation. </summary>
    ///
 

    [InitializeOnLoad]
    public class IntegrationTestsHierarchyAnnotation {

        /// <summary>   Static constructor. </summary>
        ///
     

        static IntegrationTestsHierarchyAnnotation()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DoAnnotationGUI;
        }

        /// <summary>   Executes the annotation graphical user interface operation. </summary>
        ///
     
        ///
        /// <param name="id">   The identifier. </param>
        /// <param name="rect"> The rectangle. </param>

        public static void DoAnnotationGUI(int id, Rect rect)
        {
            var obj = EditorUtility.InstanceIDToObject(id) as GameObject;
            if(!obj) return;
            
            var tc = obj.GetComponent<TestComponent>();
            if(!tc) return;
            
            if (!EditorApplication.isPlayingOrWillChangePlaymode
                && rect.Contains(Event.current.mousePosition)
                && Event.current.type == EventType.MouseDown
                && Event.current.button == 1)
            {
                IntegrationTestRendererBase.DrawContextMenu(tc);
                Event.current.Use ();
            }
            
            EditorGUIUtility.SetIconSize(new Vector2(15, 15));
            var result = IntegrationTestsRunnerWindow.GetResultForTest(tc);
            if (result != null)
            {
                var icon = result.Executed ? IntegrationTestRendererBase.GetIconForResult(result.resultType) : Icons.UnknownImg;
                EditorGUI.LabelField(new Rect(rect.xMax - 18, rect.yMin - 2, rect.width, rect.height), new GUIContent(icon));
            }
            EditorGUIUtility.SetIconSize(Vector2.zero);
        }
    }

}