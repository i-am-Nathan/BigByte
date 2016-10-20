// file:	Assets\UnityTestTools\Assertions\Comparers\Vector3Comparer.cs
//
// summary:	Implements the vector 3 comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A vector 3 comparer. </summary>
    ///
 

    public class Vector3Comparer : VectorComparerBase<Vector3>
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareType
        {
            /// <summary>   An enum constant representing the magnitude equals option. </summary>
            MagnitudeEquals,
            /// <summary>   An enum constant representing the magnitude not equals option. </summary>
            MagnitudeNotEquals
        }

        /// <summary>   Type of the compare. </summary>
        public CompareType compareType;
        /// <summary>   The floating point error. </summary>
        public double floatingPointError = 0.0001f;

        /// <summary>   Compares two T1 objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="a">    T 1 to be compared. </param>
        /// <param name="b">    T 2 to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(Vector3 a, Vector3 b)
        {
            switch (compareType)
            {
                case CompareType.MagnitudeEquals:
                    return AreVectorMagnitudeEqual(a.magnitude,
                                                   b.magnitude, floatingPointError);
                case CompareType.MagnitudeNotEquals:
                    return !AreVectorMagnitudeEqual(a.magnitude,
                                                    b.magnitude, floatingPointError);
            }
            throw new Exception();
        }
    }
}
