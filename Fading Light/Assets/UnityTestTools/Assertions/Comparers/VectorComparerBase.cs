// file:	Assets\UnityTestTools\Assertions\Comparers\VectorComparerBase.cs
//
// summary:	Implements the vector comparer base class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A vector comparer base. </summary>
    ///
 
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>

    public abstract class VectorComparerBase<T> : ComparerBaseGeneric<T>
    {
        /// <summary>   Determine if we are vector magnitude equal. </summary>
        ///
     
        ///
        /// <param name="a">                    The float to process. </param>
        /// <param name="b">                    The float to process. </param>
        /// <param name="floatingPointError">   The floating point error. </param>
        ///
        /// <returns>   True if vector magnitude equal, false if not. </returns>

        protected bool AreVectorMagnitudeEqual(float a, float b, double floatingPointError)
        {
            if (Math.Abs(a) < floatingPointError && Math.Abs(b) < floatingPointError)
                return true;
            if (Math.Abs(a - b) < floatingPointError)
                return true;
            return false;
        }
    }
}
