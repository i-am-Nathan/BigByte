// file:	Assets\UnityTestTools\Common\ITestResult.cs
//
// summary:	Declares the ITestResult interface

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTest;

/// <summary>   Interface for test result. </summary>
///
/// <remarks>    . </remarks>

public interface ITestResult
{
    /// <summary>   Gets the state of the result. </summary>
    ///
    /// <value> The result state. </value>

    TestResultState ResultState { get; }

    /// <summary>   Gets the message. </summary>
    ///
    /// <value> The message. </value>

    string Message { get; }

    /// <summary>   Gets the logs. </summary>
    ///
    /// <value> The logs. </value>

    string Logs { get; }

    /// <summary>   Gets a value indicating whether the executed. </summary>
    ///
    /// <value> True if executed, false if not. </value>

    bool Executed { get; }

    /// <summary>   Gets the name. </summary>
    ///
    /// <value> The name. </value>

    string Name { get; }

    /// <summary>   Gets the name of the full. </summary>
    ///
    /// <value> The name of the full. </value>

    string FullName { get; }

    /// <summary>   Gets the identifier. </summary>
    ///
    /// <value> The identifier. </value>

    string Id { get; }

    /// <summary>   Gets a value indicating whether this object is success. </summary>
    ///
    /// <value> True if this object is success, false if not. </value>

    bool IsSuccess { get; }

    /// <summary>   Gets the duration. </summary>
    ///
    /// <value> The duration. </value>

    double Duration { get; }

    /// <summary>   Gets the stack trace. </summary>
    ///
    /// <value> The stack trace. </value>

    string StackTrace { get; }

    /// <summary>   Gets a value indicating whether this object is ignored. </summary>
    ///
    /// <value> True if this object is ignored, false if not. </value>

    bool IsIgnored { get; }
}
