// file:	Assets\UnityTestTools\Assertions\Editor\GroupByTestsRenderer.cs
//
// summary:	Implements the group by tests renderer class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A group by tests renderer. </summary>
    ///
 

    public class GroupByTestsRenderer : AssertionListRenderer<GameObject>
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
            return assertionComponents.GroupBy(c =>
                                               {
                                                   var temp = c.transform;
                                                   while (temp != null)
                                                   {
                                                       if (temp.GetComponent("TestComponent") != null) return c.gameObject;
                                                       temp = temp.parent.transform;
                                                   }
                                                   return null;
                                               });
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
