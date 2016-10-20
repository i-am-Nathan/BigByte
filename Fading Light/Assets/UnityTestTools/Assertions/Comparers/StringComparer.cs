// file:	Assets\UnityTestTools\Assertions\Comparers\StringComparer.cs
//
// summary:	Implements the string comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A string comparer. </summary>
    ///
 

    public class StringComparer : ComparerBaseGeneric<string>
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareType
        {
            /// <summary>   An enum constant representing the equal option. </summary>
            Equal,
            /// <summary>   An enum constant representing the not equal option. </summary>
            NotEqual,
            /// <summary>   An enum constant representing the shorter option. </summary>
            Shorter,
            /// <summary>   An enum constant representing the longer option. </summary>
            Longer
        }

        /// <summary>   Type of the compare. </summary>
        public CompareType compareType;
        /// <summary>   Type of the comparison. </summary>
        public StringComparison comparisonType = StringComparison.Ordinal;
        /// <summary>   True to ignore case. </summary>
        public bool ignoreCase = false;

        /// <summary>   Compares two T1 objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="a">    T 1 to be compared. </param>
        /// <param name="b">    T 2 to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(string a, string b)
        {
            if (ignoreCase)
            {
                a = a.ToLower();
                b = b.ToLower();
            }
            switch (compareType)
            {
                case CompareType.Equal:
                    return String.Compare(a, b, comparisonType) == 0;
                case CompareType.NotEqual:
                    return String.Compare(a, b, comparisonType) != 0;
                case CompareType.Longer:
                    return String.Compare(a, b, comparisonType) > 0;
                case CompareType.Shorter:
                    return String.Compare(a, b, comparisonType) < 0;
            }
            throw new Exception();
        }
    }
}
