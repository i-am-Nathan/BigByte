// file:	Assets\UnityTestTools\Assertions\Comparers\IsRenderedByCamera.cs
//
// summary:	Implements the is rendered by camera class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   The is rendered by camera. </summary>
    ///
 

    public class IsRenderedByCamera : ComparerBaseGeneric<Renderer, Camera>
    {
        /// <summary>   Values that represent compare types. </summary>
        ///
     

        public enum CompareType
        {
            /// <summary>   An enum constant representing the is visible option. </summary>
            IsVisible,
            /// <summary>   An enum constant representing the is not visible option. </summary>
            IsNotVisible,
        };

        /// <summary>   Type of the compare. </summary>
        public CompareType compareType;

        /// <summary>   Compares two T1 objects to determine their relative ordering. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="renderer"> T 1 to be compared. </param>
        /// <param name="camera">   T 2 to be compared. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        protected override bool Compare(Renderer renderer, Camera camera)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            var isVisible = GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
            switch (compareType)
            {
                case CompareType.IsVisible:
                    return isVisible;
                case CompareType.IsNotVisible:
                    return !isVisible;
            }
            throw new Exception();
        }
    }
}
