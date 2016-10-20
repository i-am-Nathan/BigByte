// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\IntegrationTestsRunnerSettings.cs
//
// summary:	Implements the integration tests runner settings class

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityTest
{
    /// <summary>   An integration tests runner settings. </summary>
    ///
 

    public class IntegrationTestsRunnerSettings : ProjectSettingsBase
    {
        /// <summary>   True to block user interface when running. </summary>
        public bool blockUIWhenRunning = true;
        /// <summary>   True to pause on test failure. </summary>
        public bool pauseOnTestFailure;

        /// <summary>   Toggle block user interface when running. </summary>
        ///
     

        public void ToggleBlockUIWhenRunning ()
        {
            blockUIWhenRunning = !blockUIWhenRunning;
            Save ();
        }

        /// <summary>   Toggle pause on test failure. </summary>
        ///
     

        public void TogglePauseOnTestFailure()
        {
            pauseOnTestFailure = !pauseOnTestFailure;
            Save ();
        }
    }
}
