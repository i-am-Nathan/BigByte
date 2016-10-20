// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\PlatformRunner\PlatformRunnerSettings.cs
//
// summary:	Implements the platform runner settings class

using System;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A platform runner settings. </summary>
    ///
 

    public class PlatformRunnerSettings : ProjectSettingsBase
    {
        /// <summary>   Full pathname of the results file. </summary>
        public string resultsPath;
        /// <summary>   True to send results over network. </summary>
        public bool sendResultsOverNetwork = true;
        /// <summary>   The port. </summary>
        public int port = 0;
    }
}
