// file:	Assets\UnityTestTools\Examples\IntegrationTestsFrameworkExamples\DynamicIntegrationTest.cs
//
// summary:	Implements the dynamic integration test class

using System;
using System.Collections.Generic;
using UnityEngine;

[IntegrationTest.DynamicTestAttribute("ExampleIntegrationTests")]
// [IntegrationTest.Ignore]

/// <summary>   A dynamic integration test. </summary>
///
/// <remarks>    . </remarks>

[IntegrationTest.ExpectExceptions(false, typeof(ArgumentException))]
[IntegrationTest.SucceedWithAssertions]
[IntegrationTest.TimeoutAttribute(1)]
[IntegrationTest.ExcludePlatformAttribute(RuntimePlatform.Android, RuntimePlatform.LinuxPlayer)]
public class DynamicIntegrationTest : MonoBehaviour
{
    /// <summary>   Starts this object. </summary>
    ///
 

    public void Start()
    {
        IntegrationTest.Pass(gameObject);
    }
}
