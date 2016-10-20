// file:	Assets\UnityTestTools\Assertions\Comparers\BoolComparer.cs
//
// summary:	Implements the comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A comparer. </summary>
    ///
 

    public class BoolComparer : ComparerBaseGeneric<bool>
    {
        /// <summary>   Compares two bool objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <param name="a">    Bool to be compared. </param>
        /// <param name="b">    Bool to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(bool a, bool b)
        {
            return a == b;
        }
    }
}
