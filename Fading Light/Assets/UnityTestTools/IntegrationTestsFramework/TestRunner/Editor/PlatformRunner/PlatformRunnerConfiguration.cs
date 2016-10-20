// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\PlatformRunner\PlatformRunnerConfiguration.cs
//
// summary:	Implements the platform runner configuration class

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>   (Serializable) a platform runner configuration. </summary>
///
/// <remarks>    . </remarks>

[Serializable]
public class PlatformRunnerConfiguration
{
    /// <summary>   The build scenes. </summary>
    public List<string> buildScenes;
    /// <summary>   The test scenes. </summary>
    public List<string> testScenes;
    /// <summary>   The build target. </summary>
    public BuildTarget buildTarget;
    /// <summary>   True to run in editor. </summary>
    public bool runInEditor;
    /// <summary>   Name of the project. </summary>
    public string projectName = SceneManager.GetActiveScene().path;

    /// <summary>   The results dir. </summary>
    public string resultsDir = null;
    /// <summary>   True to send results over network. </summary>
    public bool sendResultsOverNetwork;
    /// <summary>   List of ips. </summary>
    public List<string> ipList;
    /// <summary>   The port. </summary>
    public int port;

    /// <summary>   Constructor. </summary>
    ///
 
    ///
    /// <param name="buildTarget">  The build target. </param>

    public PlatformRunnerConfiguration(BuildTarget buildTarget)
    {
        this.buildTarget = buildTarget;
        projectName = SceneManager.GetActiveScene().path;
    }

    /// <summary>   Default constructor. </summary>
    ///
 

    public PlatformRunnerConfiguration()
        : this(BuildTarget.StandaloneWindows)
    {
    }

    /// <summary>   Gets temporary path. </summary>
    ///
 
    ///
    /// <returns>   The temporary path. </returns>

    public string GetTempPath()
    {
        if (string.IsNullOrEmpty(projectName))
            projectName = Path.GetTempFileName();

        var path = Path.Combine("Temp", projectName);
        switch (buildTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return path + ".exe";
            case BuildTarget.StandaloneOSXIntel:
                return path + ".app";
            case BuildTarget.Android:
                return path + ".apk";
            default:
                if (buildTarget.ToString() == "BlackBerry" || buildTarget.ToString() == "BB10")
                    return path + ".bar";
                return path;
        }
    }

    /// <summary>   Gets connection i ps. </summary>
    ///
 
    ///
    /// <returns>   An array of string. </returns>

    public string[] GetConnectionIPs()
    {
        return ipList.Select(ip => ip + ":" + port).ToArray();
    }

    /// <summary>   Try to get free port. </summary>
    ///
 
    ///
    /// <returns>   An int. </returns>

    public static int TryToGetFreePort()
    {
        var port = -1;
        try
        {
            var l = new TcpListener(IPAddress.Any, 0);
            l.Start();
            port = ((IPEndPoint)l.Server.LocalEndPoint).Port;
            l.Stop();
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
        }
        return port;
    }
}
