// file:	Assets\UnityTestTools\Assertions\Comparers\TransformComparer.cs
//
// summary:	Implements the transform comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A transform comparer. </summary>
    ///
 

    public class TransformComparer : ComparerBaseGeneric<Transform>
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareType { Equals, NotEquals }

        /// <summary>   Type of the compare. </summary>
        public CompareType compareType;

        /// <summary>   Compares two T1 objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="a">    T 1 to be compared. </param>
        /// <param name="b">    T 2 to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(Transform a, Transform b)
        {
            if (compareType == CompareType.Equals)
            {
                return a.position == b.position;
            }
            if (compareType == CompareType.NotEquals)
            {
                return a.position != b.position;
            }
            throw new Exception();
        }
    }
}
