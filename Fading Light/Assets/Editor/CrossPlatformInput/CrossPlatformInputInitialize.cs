// file:	Assets\Editor\CrossPlatformInput\CrossPlatformInputInitialize.cs
//
// summary:	Implements the cross platform input initialize class

using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnityStandardAssets.CrossPlatformInput.Inspector
{
    /// <summary>   The cross platform initialize. </summary>
    ///
 

    [InitializeOnLoad]
    public class CrossPlatformInitialize
    {
        // Custom compiler defines:
        //
        // CROSS_PLATFORM_INPUT : denotes that cross platform input package exists, so that other packages can use their CrossPlatformInput functions.
        // EDITOR_MOBILE_INPUT : denotes that mobile input should be used in editor, if a mobile build target is selected. (i.e. using Unity Remote app).
        // MOBILE_INPUT : denotes that mobile input should be used right now!

        /// <summary>   Static constructor. </summary>
        ///
     

        static CrossPlatformInitialize()
        {
            var defines = GetDefinesList(buildTargetGroups[0]);
            if (!defines.Contains("CROSS_PLATFORM_INPUT"))
            {
                SetEnabled("CROSS_PLATFORM_INPUT", true, false);
                SetEnabled("MOBILE_INPUT", true, true);
            }
        }

        /// <summary>   Enables this object. </summary>
        ///
     

        [MenuItem("Mobile Input/Enable")]
        private static void Enable()
        {
            SetEnabled("MOBILE_INPUT", true, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                case BuildTarget.iOS:
                case BuildTarget.WP8Player:
                case BuildTarget.PSM: 
                case BuildTarget.WSAPlayer:
                    EditorUtility.DisplayDialog("Mobile Input",
                                                "You have enabled Mobile Input. You'll need to use the Unity Remote app on a connected device to control your game in the Editor.",
                                                "OK");
                    break;

                default:
                    EditorUtility.DisplayDialog("Mobile Input",
                                                "You have enabled Mobile Input, but you have a non-mobile build target selected in your build settings. The mobile control rigs won't be active or visible on-screen until you switch the build target to a mobile platform.",
                                                "OK");
                    break;
            }
        }

        /// <summary>   Enables the validate. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        [MenuItem("Mobile Input/Enable", true)]
        private static bool EnableValidate()
        {
            var defines = GetDefinesList(mobileBuildTargetGroups[0]);
            return !defines.Contains("MOBILE_INPUT");
        }

        /// <summary>   Disables this object. </summary>
        ///
     

        [MenuItem("Mobile Input/Disable")]
        private static void Disable()
        {
            SetEnabled("MOBILE_INPUT", false, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                case BuildTarget.iOS:
                case BuildTarget.WP8Player:
                    EditorUtility.DisplayDialog("Mobile Input",
                                                "You have disabled Mobile Input. Mobile control rigs won't be visible, and the Cross Platform Input functions will always return standalone controls.",
                                                "OK");
                    break;
            }
        }

        /// <summary>   Disables the validate. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        [MenuItem("Mobile Input/Disable", true)]
        private static bool DisableValidate()
        {
            var defines = GetDefinesList(mobileBuildTargetGroups[0]);
            return defines.Contains("MOBILE_INPUT");
        }


        /// <summary>   Groups the build target belongs to. </summary>
        private static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
            {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.WebPlayer,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WP8
            };

        /// <summary>   Groups the mobile build target belongs to. </summary>
        private static BuildTargetGroup[] mobileBuildTargetGroups = new BuildTargetGroup[]
            {
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WP8,
                BuildTargetGroup.PSM, 
                BuildTargetGroup.SamsungTV,
                BuildTargetGroup.Tizen,
                BuildTargetGroup.WSA 
            };

        /// <summary>   Sets an enabled. </summary>
        ///
     
        ///
        /// <param name="defineName">   Name of the define. </param>
        /// <param name="enable">       True to enable, false to disable. </param>
        /// <param name="mobile">       True to mobile. </param>

        private static void SetEnabled(string defineName, bool enable, bool mobile)
        {
            //Debug.Log("setting "+defineName+" to "+enable);
            foreach (var group in mobile ? mobileBuildTargetGroups : buildTargetGroups)
            {
                var defines = GetDefinesList(group);
                if (enable)
                {
                    if (defines.Contains(defineName))
                    {
                        return;
                    }
                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                    {
                        return;
                    }
                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }
                string definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }

        /// <summary>   Gets defines list. </summary>
        ///
     
        ///
        /// <param name="group">    The group. </param>
        ///
        /// <returns>   The defines list. </returns>

        private static List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
    }
}
