// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\Renderer\RenderingOptions.cs
//
// summary:	Implements the rendering options class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A rendering options. </summary>
    ///
 

    public class RenderingOptions
    {
        /// <summary>   A filter specifying the name. </summary>
        public string nameFilter;
        /// <summary>   True to show, false to hide the succeeded. </summary>
        public bool showSucceeded;
        /// <summary>   True to show, false to hide the failed. </summary>
        public bool showFailed;
        /// <summary>   True to show, false to hide the ignored. </summary>
        public bool showIgnored;
        /// <summary>   True to show, false to hide the not runned. </summary>
        public bool showNotRunned;
        /// <summary>   The categories. </summary>
        public string[] categories;
    }
}
