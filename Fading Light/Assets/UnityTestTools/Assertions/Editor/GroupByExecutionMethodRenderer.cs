// file:	Assets\UnityTestTools\Assertions\Editor\GroupByExecutionMethodRenderer.cs
//
// summary:	Implements the group by execution method renderer class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A group by execution method renderer. </summary>
    ///
 

    public class GroupByExecutionMethodRenderer : AssertionListRenderer<CheckMethod>
    {
        /// <summary>   Enumerates group result in this collection. </summary>
        ///
     
        ///
        /// <param name="assertionComponents">  The assertion components. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process group result in this collection.
        /// </returns>

        protected override IEnumerable<IGrouping<CheckMethod, AssertionComponent>> GroupResult(IEnumerable<AssertionComponent> assertionComponents)
        {
            var enumVals = Enum.GetValues(typeof(CheckMethod)).Cast<CheckMethod>();
            var pairs = new List<CheckFunctionAssertionPair>();

            foreach (var checkMethod in enumVals)
            {
                var components = assertionComponents.Where(c => (c.checkMethods & checkMethod) == checkMethod);
                var componentPairs = components.Select(a => new CheckFunctionAssertionPair {checkMethod = checkMethod, assertionComponent = a});
                pairs.AddRange(componentPairs);
            }
            return pairs.GroupBy(pair => pair.checkMethod,
                                 pair => pair.assertionComponent);
        }

        /// <summary>   A check function assertion pair. </summary>
        ///
     

        private class CheckFunctionAssertionPair
        {
            /// <summary>   The assertion component. </summary>
            public AssertionComponent assertionComponent;
            /// <summary>   The check method. </summary>
            public CheckMethod checkMethod;
        }
    }
}
