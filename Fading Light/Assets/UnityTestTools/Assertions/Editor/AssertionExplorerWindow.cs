// file:	Assets\UnityTestTools\Assertions\Editor\AssertionExplorerWindow.cs
//
// summary:	Implements the assertion explorer Windows Form

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_METRO
#warning Assertion component is not supported on Windows Store apps
#endif

namespace UnityTest
{
    /// <summary>   (Serializable) form for viewing the assertion explorer. </summary>
    ///
 

    [Serializable]
    public class AssertionExplorerWindow : EditorWindow
    {
        /// <summary>   all assertions. </summary>
        private List<AssertionComponent> m_AllAssertions = new List<AssertionComponent>();
        /// <summary>   The filter text. </summary>
        [SerializeField]
        private string m_FilterText = "";
        /// <summary>   Type of the filter. </summary>
        [SerializeField]
        private FilterType m_FilterType;
        /// <summary>   The fold markers. </summary>
        [SerializeField]
        private List<string> m_FoldMarkers = new List<string>();
        /// <summary>   Amount to group by. </summary>
        [SerializeField]
        private GroupByType m_GroupBy;
        /// <summary>   The scroll position. </summary>
        [SerializeField]
        private Vector2 m_ScrollPosition = Vector2.zero;
        /// <summary>   The next reload Date/Time. </summary>
        private DateTime m_NextReload = DateTime.Now;
        /// <summary>   True if should reload. </summary>
        [SerializeField]
        private static bool s_ShouldReload;
        /// <summary>   Type of the show. </summary>
        [SerializeField]
        private ShowType m_ShowType;

        /// <summary>   Default constructor. </summary>
        ///
     

        public AssertionExplorerWindow()
        {
            titleContent = new GUIContent("Assertion Explorer");
        }

        /// <summary>   Executes the did open scene action. </summary>
        ///
     

        public void OnDidOpenScene()
        {
            ReloadAssertionList();
        }

        /// <summary>   Executes the focus action. </summary>
        ///
     

        public void OnFocus()
        {
            ReloadAssertionList();
        }

        /// <summary>   Reload assertion list. </summary>
        ///
     

        private void ReloadAssertionList()
        {
            m_NextReload = DateTime.Now.AddSeconds(1);
            s_ShouldReload = true;
        }

        /// <summary>   Executes the hierarchy change action. </summary>
        ///
     

        public void OnHierarchyChange()
        {
            ReloadAssertionList();
        }

        /// <summary>   Executes the inspector update action. </summary>
        ///
     

        public void OnInspectorUpdate()
        {
            if (s_ShouldReload && m_NextReload < DateTime.Now)
            {
                s_ShouldReload = false;
                m_AllAssertions = new List<AssertionComponent>((AssertionComponent[])Resources.FindObjectsOfTypeAll(typeof(AssertionComponent)));
                Repaint();
            }
        }

        /// <summary>   Executes the graphical user interface action. </summary>
        ///
     

        public void OnGUI()
        {
            DrawMenuPanel();

            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
            if (m_AllAssertions != null)
                GetResultRendere().Render(FilterResults(m_AllAssertions, m_FilterText.ToLower()), m_FoldMarkers);
            EditorGUILayout.EndScrollView();
        }

        /// <summary>   Enumerates filter results in this collection. </summary>
        ///
     
        ///
        /// <param name="assertionComponents">  The assertion components. </param>
        /// <param name="text">                 The text. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process filter results in this collection.
        /// </returns>

        private IEnumerable<AssertionComponent> FilterResults(List<AssertionComponent> assertionComponents, string text)
        {
            if (m_ShowType == ShowType.ShowDisabled)
                assertionComponents = assertionComponents.Where(c => !c.enabled).ToList();
            else if (m_ShowType == ShowType.ShowEnabled)
                assertionComponents = assertionComponents.Where(c => c.enabled).ToList();

            if (string.IsNullOrEmpty(text))
                return assertionComponents;

            switch (m_FilterType)
            {
                case FilterType.ComparerName:
                    return assertionComponents.Where(c => c.Action.GetType().Name.ToLower().Contains(text));
                case FilterType.AttachedGameObject:
                    return assertionComponents.Where(c => c.gameObject.name.ToLower().Contains(text));
                case FilterType.FirstComparedGameObjectPath:
                    return assertionComponents.Where(c => c.Action.thisPropertyPath.ToLower().Contains(text));
                case FilterType.FirstComparedGameObject:
                    return assertionComponents.Where(c => c.Action.go != null
                                                     && c.Action.go.name.ToLower().Contains(text));
                case FilterType.SecondComparedGameObjectPath:
                    return assertionComponents.Where(c =>
                                                     c.Action is ComparerBase
                                                     && (c.Action as ComparerBase).otherPropertyPath.ToLower().Contains(text));
                case FilterType.SecondComparedGameObject:
                    return assertionComponents.Where(c =>
                                                     c.Action is ComparerBase
                                                     && (c.Action as ComparerBase).other != null
                                                     && (c.Action as ComparerBase).other.name.ToLower().Contains(text));
                default:
                    return assertionComponents;
            }
        }

        /// <summary>   The group by comparer renderer. </summary>
        private readonly IListRenderer m_GroupByComparerRenderer = new GroupByComparerRenderer();
        /// <summary>   The group by execution method renderer. </summary>
        private readonly IListRenderer m_GroupByExecutionMethodRenderer = new GroupByExecutionMethodRenderer();
        /// <summary>   The group by go renderer. </summary>
        private readonly IListRenderer m_GroupByGoRenderer = new GroupByGoRenderer();
        /// <summary>   The group by tests renderer. </summary>
        private readonly IListRenderer m_GroupByTestsRenderer = new GroupByTestsRenderer();
        /// <summary>   The group by nothing renderer. </summary>
        private readonly IListRenderer m_GroupByNothingRenderer = new GroupByNothingRenderer();

        /// <summary>   Gets result rendere. </summary>
        ///
     
        ///
        /// <returns>   The result rendere. </returns>

        private IListRenderer GetResultRendere()
        {
            switch (m_GroupBy)
            {
                case GroupByType.Comparer:
                    return m_GroupByComparerRenderer;
                case GroupByType.ExecutionMethod:
                    return m_GroupByExecutionMethodRenderer;
                case GroupByType.GameObjects:
                    return m_GroupByGoRenderer;
                case GroupByType.Tests:
                    return m_GroupByTestsRenderer;
                default:
                    return m_GroupByNothingRenderer;
            }
        }

        /// <summary>   Draw menu panel. </summary>
        ///
     

        private void DrawMenuPanel()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.LabelField("Group by:", Styles.toolbarLabel, GUILayout.MaxWidth(60));
            m_GroupBy = (GroupByType)EditorGUILayout.EnumPopup(m_GroupBy, EditorStyles.toolbarPopup, GUILayout.MaxWidth(150));

            GUILayout.FlexibleSpace();

            m_ShowType = (ShowType)EditorGUILayout.EnumPopup(m_ShowType, EditorStyles.toolbarPopup, GUILayout.MaxWidth(100));

            EditorGUILayout.LabelField("Filter by:", Styles.toolbarLabel, GUILayout.MaxWidth(50));
            m_FilterType = (FilterType)EditorGUILayout.EnumPopup(m_FilterType, EditorStyles.toolbarPopup, GUILayout.MaxWidth(100));
            m_FilterText = GUILayout.TextField(m_FilterText, "ToolbarSeachTextField", GUILayout.MaxWidth(100));
            if (GUILayout.Button(GUIContent.none, string.IsNullOrEmpty(m_FilterText) ? "ToolbarSeachCancelButtonEmpty" : "ToolbarSeachCancelButton", GUILayout.ExpandWidth(false)))
                m_FilterText = "";
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>   Shows the window. </summary>
        ///
     
        ///
        /// <returns>   An AssertionExplorerWindow. </returns>

        [MenuItem("Unity Test Tools/Assertion Explorer")]
        public static AssertionExplorerWindow ShowWindow()
        {
            var w = GetWindow(typeof(AssertionExplorerWindow));
            w.Show();
            return w as AssertionExplorerWindow;
        }

        /// <summary>   Values that represent filter types. </summary>
        ///
     

        private enum FilterType
        {
            /// <summary>   An enum constant representing the comparer name option. </summary>
            ComparerName,
            /// <summary>   An enum constant representing the first compared game object option. </summary>
            FirstComparedGameObject,
            /// <summary>   An enum constant representing the first compared game object path option. </summary>
            FirstComparedGameObjectPath,
            /// <summary>   An enum constant representing the second compared game object option. </summary>
            SecondComparedGameObject,
            /// <summary>   An enum constant representing the second compared game object path option. </summary>
            SecondComparedGameObjectPath,
            /// <summary>   An enum constant representing the attached game object option. </summary>
            AttachedGameObject
        }

        /// <summary>   Values that represent show types. </summary>
        ///
     

        private enum ShowType
        {
            /// <summary>   An enum constant representing the show all option. </summary>
            ShowAll,
            /// <summary>   An enum constant representing the show enabled option. </summary>
            ShowEnabled,
            /// <summary>   An enum constant representing the show disabled option. </summary>
            ShowDisabled
        }

        /// <summary>   Values that represent group by types. </summary>
        ///
     

        private enum GroupByType
        {
            /// <summary>   An enum constant representing the nothing option. </summary>
            Nothing,
            /// <summary>   An enum constant representing the comparer option. </summary>
            Comparer,
            /// <summary>   An enum constant representing the game objects option. </summary>
            GameObjects,
            /// <summary>   An enum constant representing the execution method option. </summary>
            ExecutionMethod,
            /// <summary>   An enum constant representing the tests option. </summary>
            Tests
        }

        /// <summary>   Reloads this object. </summary>
        ///
     

        public static void Reload()
        {
            s_ShouldReload = true;
        }
    }
}
