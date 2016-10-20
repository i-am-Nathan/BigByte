// file:	Assets\UnityTestTools\Assertions\Editor\GroupByComparerRenderer.cs
//
// summary:	Implements the group by comparer renderer class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A group by comparer renderer. </summary>
    ///
 

    public class GroupByComparerRenderer : AssertionListRenderer<Type>
    {
        /// <summary>   Enumerates group result in this collection. </summary>
        ///
     
        ///
        /// <param name="assertionComponents">  The assertion components. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process group result in this collection.
        /// </returns>

        protected override IEnumerable<IGrouping<Type, AssertionComponent>> GroupResult(IEnumerable<AssertionComponent> assertionComponents)
        {
            return assertionComponents.GroupBy(c => c.Action.GetType());
        }

        /// <summary>   Gets string key. </summary>
        ///
     
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The string key. </returns>

        protected override string GetStringKey(Type key)
        {
            return key.Name;
        }
    }
}
