// file:	Assets\UnityTestTools\Assertions\Comparers\FloatComparer.cs
//
// summary:	Implements the float comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A float comparer. </summary>
    ///
 

    public class FloatComparer : ComparerBaseGeneric<float>
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareTypes
        {
            /// <summary>   An enum constant representing the equal option. </summary>
            Equal,
            /// <summary>   An enum constant representing the not equal option. </summary>
            NotEqual,
            /// <summary>   An enum constant representing the greater option. </summary>
            Greater,
            /// <summary>   An enum constant representing the less option. </summary>
            Less
        }

        /// <summary>   List of types of the compares. </summary>
        public CompareTypes compareTypes;
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

        protected override bool Compare(float a, float b)
        {
            switch (compareTypes)
            {
                case CompareTypes.Equal:
                    return Math.Abs(a - b) < floatingPointError;
                case CompareTypes.NotEqual:
                    return Math.Abs(a - b) > floatingPointError;
                case CompareTypes.Greater:
                    return a > b;
                case CompareTypes.Less:
                    return a < b;
            }
            throw new Exception();
        }

        /// <summary>   Gets depth of search. </summary>
        ///
     
        ///
        /// <returns>   The depth of search. </returns>

        public override int GetDepthOfSearch()
        {
            return 3;
        }
    }
}
