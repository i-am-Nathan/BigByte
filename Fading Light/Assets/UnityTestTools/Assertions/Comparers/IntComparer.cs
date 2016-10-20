// file:	Assets\UnityTestTools\Assertions\Comparers\IntComparer.cs
//
// summary:	Implements the int comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   An int comparer. </summary>
    ///
 

    public class IntComparer : ComparerBaseGeneric<int>
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareType
        {
            /// <summary>   An enum constant representing the equal option. </summary>
            Equal,
            /// <summary>   An enum constant representing the not equal option. </summary>
            NotEqual,
            /// <summary>   An enum constant representing the greater option. </summary>
            Greater,
            /// <summary>   An enum constant representing the greater or equal option. </summary>
            GreaterOrEqual,
            /// <summary>   An enum constant representing the less option. </summary>
            Less,
            /// <summary>   An enum constant representing the less or equal option. </summary>
            LessOrEqual
        };

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

        protected override bool Compare(int a, int b)
        {
            switch (compareType)
            {
                case CompareType.Equal:
                    return a == b;
                case CompareType.NotEqual:
                    return a != b;
                case CompareType.Greater:
                    return a > b;
                case CompareType.GreaterOrEqual:
                    return a >= b;
                case CompareType.Less:
                    return a < b;
                case CompareType.LessOrEqual:
                    return a <= b;
            }
            throw new Exception();
        }
    }
}
