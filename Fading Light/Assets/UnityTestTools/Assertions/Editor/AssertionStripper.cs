// file:	Assets\UnityTestTools\Assertions\Editor\AssertionStripper.cs
//
// summary:	Implements the assertion stripper class

using System;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityTest;
using Object = UnityEngine.Object;

/// <summary>   An assertion stripper. </summary>
///
/// <remarks>    . </remarks>

public class AssertionStripper
{
    /// <summary>   Executes the postprocess scene action. </summary>
    ///
 

    [PostProcessScene]
    public static void OnPostprocessScene()
    {
        if (Debug.isDebugBuild) return;
        RemoveAssertionsFromGameObjects();
    }

    /// <summary>   Removes the assertions from game objects. </summary>
    ///
 

    private static void RemoveAssertionsFromGameObjects()
    {
        var allAssertions = Resources.FindObjectsOfTypeAll(typeof(AssertionComponent)) as AssertionComponent[];
        foreach (var assertion in allAssertions)
        {
            Object.DestroyImmediate(assertion);
        }
    }
}
