// file:	Assets\UnityTestTools\Assertions\Comparers\GeneralComparer.cs
//
// summary:	Implements the general comparer class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A general comparer. </summary>
    ///
 

    public class GeneralComparer : ComparerBase
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareType { AEqualsB, ANotEqualsB }

        /// <summary>   Type of the compare. </summary>
        public CompareType compareType;

        /// <summary>
        /// Compares this object object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="a">    Object to be compared. </param>
        /// <param name="b">    Object to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(object a, object b)
        {
            if (compareType == CompareType.AEqualsB)
                return a.Equals(b);
            if (compareType == CompareType.ANotEqualsB)
                return !a.Equals(b);
            throw new Exception();
        }
    }
}
