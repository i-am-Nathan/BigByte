// file:	Assets\UnityTestTools\Assertions\Editor\GroupByNothingRenderer.cs
//
// summary:	Implements the group by nothing renderer class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A group by nothing renderer. </summary>
    ///
 

    public class GroupByNothingRenderer : AssertionListRenderer<string>
    {
        /// <summary>   Enumerates group result in this collection. </summary>
        ///
     
        ///
        /// <param name="assertionComponents">  The assertion components. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process group result in this collection.
        /// </returns>

        protected override IEnumerable<IGrouping<string, AssertionComponent>> GroupResult(IEnumerable<AssertionComponent> assertionComponents)
        {
            return assertionComponents.GroupBy(c => "");
        }

        /// <summary>   Gets string key. </summary>
        ///
     
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The string key. </returns>

        protected override string GetStringKey(string key)
        {
            return "";
        }
    }
}
