// file:	Assets\UnityTestTools\Examples\IntegrationTestsFrameworkExamples\ThrowCustomException.cs
//
// summary:	Implements the throw custom exception class

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>   Exception for signalling throw custom errors. </summary>
///
/// <remarks>    . </remarks>

public class ThrowCustomException : MonoBehaviour
{
    /// <summary>   Starts this object. </summary>
    ///
 
    ///
    /// <exception cref="CustomException">  Thrown when a Custom error condition occurs. </exception>

    public void Start()
    {
        throw new CustomException();
    }

    /// <summary>   Exception for signalling custom errors. </summary>
    ///
 

    private class CustomException : Exception
    {
    }
}
