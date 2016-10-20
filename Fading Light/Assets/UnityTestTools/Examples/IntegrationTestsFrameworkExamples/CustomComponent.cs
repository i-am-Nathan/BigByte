// file:	Assets\UnityTestTools\Examples\IntegrationTestsFrameworkExamples\CustomComponent.cs
//
// summary:	Implements the custom component class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A custom component. </summary>
    ///
 

    public class CustomComponent : MonoBehaviour
    {
        /// <summary>   Gets or sets my float property. </summary>
        ///
        /// <value> my float property. </value>

        public float MyFloatProp { get; set; }
        /// <summary>   my float field. </summary>
        public float MyFloatField = 3;
    }
}
