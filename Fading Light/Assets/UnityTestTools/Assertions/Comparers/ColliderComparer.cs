// file:	Assets\UnityTestTools\Assertions\Comparers\ColliderComparer.cs
//
// summary:	Implements the collider comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A collider comparer. </summary>
    ///
 

    public class ColliderComparer : ComparerBaseGeneric<Bounds>
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareType
        {
            /// <summary>   An enum constant representing the intersects option. </summary>
            Intersects,
            /// <summary>   An enum constant representing the does not intersect option. </summary>
            DoesNotIntersect
        };

        /// <summary>   Type of the compare. </summary>
        public CompareType compareType;

        /// <summary>   Compares two Bounds objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="a">    Bounds to be compared. </param>
        /// <param name="b">    Bounds to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(Bounds a, Bounds b)
        {
            switch (compareType)
            {
                case CompareType.Intersects:
                    return a.Intersects(b);
                case CompareType.DoesNotIntersect:
                    return !a.Intersects(b);
            }
            throw new Exception();
        }
    }
}
