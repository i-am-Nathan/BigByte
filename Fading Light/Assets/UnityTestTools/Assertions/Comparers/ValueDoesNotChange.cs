// file:	Assets\UnityTestTools\Assertions\Comparers\ValueDoesNotChange.cs
//
// summary:	Implements the value does not change class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A value does not change. </summary>
    ///
 

    public class ValueDoesNotChange : ActionBase
    {
        /// <summary>   The value. </summary>
        private object m_Value;

        /// <summary>
        /// Compares this object object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="a">    The object to compare to this object. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(object a)
        {
            if (m_Value == null)
                m_Value = a;
            if (!m_Value.Equals(a))
                return false;
            return true;
        }
    }
}
