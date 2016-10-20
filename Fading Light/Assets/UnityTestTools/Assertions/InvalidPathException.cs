// file:	Assets\UnityTestTools\Assertions\InvalidPathException.cs
//
// summary:	Implements the invalid path exception class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   Exception for signalling invalid path errors. </summary>
    ///
 

    public class InvalidPathException : Exception
    {
        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="path"> Full pathname of the file. </param>

        public InvalidPathException(string path)
            : base("Invalid path part " + path)
        {
        }
    }
}
