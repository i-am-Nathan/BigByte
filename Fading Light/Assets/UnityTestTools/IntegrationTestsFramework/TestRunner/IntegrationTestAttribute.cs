// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\IntegrationTestAttribute.cs
//
// summary:	Implements the integration test attribute class

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>   Attribute for integration test. </summary>
///
/// <remarks>    . </remarks>

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class IntegrationTestAttribute : Attribute
{
    /// <summary>   Full pathname of the file. </summary>
    private readonly string m_Path;

    /// <summary>   Constructor. </summary>
    ///
 
    ///
    /// <param name="path"> Full pathname of the file. </param>

    public IntegrationTestAttribute(string path)
    {
        if (path.EndsWith(".unity"))
            path = path.Substring(0, path.Length - ".unity".Length);
        m_Path = path;
    }

    /// <summary>   Include on scene. </summary>
    ///
 
    ///
    /// <param name="scenePath">    Full pathname of the scene file. </param>
    ///
    /// <returns>   True if it succeeds, false if it fails. </returns>

    public bool IncludeOnScene(string scenePath)
    {
        if (scenePath == m_Path) return true;
        var fileName = Path.GetFileNameWithoutExtension(scenePath);
        return fileName == m_Path;
    }
}
