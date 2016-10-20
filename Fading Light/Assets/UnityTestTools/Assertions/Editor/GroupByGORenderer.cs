// file:	Assets\UnityTestTools\Assertions\Editor\GroupByGORenderer.cs
//
// summary:	Implements the group by go renderer class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A group by go renderer. </summary>
    ///
 

    public class GroupByGoRenderer : AssertionListRenderer<GameObject>
    {
        /// <summary>   Enumerates group result in this collection. </summary>
        ///
     
        ///
        /// <param name="assertionComponents">  The assertion components. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process group result in this collection.
        /// </returns>

        protected override IEnumerable<IGrouping<GameObject, AssertionComponent>> GroupResult(IEnumerable<AssertionComponent> assertionComponents)
        {
            return assertionComponents.GroupBy(c => c.gameObject);
        }

        /// <summary>   Print foldout. </summary>
        ///
     
        ///
        /// <param name="isFolded"> True if this object is folded. </param>
        /// <param name="key">      The key. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool PrintFoldout(bool isFolded, GameObject key)
        {
            isFolded = base.PrintFoldout(isFolded,
                                         key);

            EditorGUILayout.ObjectField(key,
                                        typeof(GameObject),
                                        true,
                                        GUILayout.ExpandWidth(false));

            return isFolded;
        }

        /// <summary>   Gets foldout display name. </summary>
        ///
     
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The foldout display name. </returns>

        protected override string GetFoldoutDisplayName(GameObject key)
        {
            return key.name;
        }
    }
}
