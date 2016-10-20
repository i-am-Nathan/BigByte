// file:	Assets\UnityTestTools\Common\Editor\ProjectSettingsBase.cs
//
// summary:	Implements the project settings base class

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A project settings base. </summary>
    ///
 

    public abstract class ProjectSettingsBase : ScriptableObject
    {
        /// <summary>   Full pathname of the settings file. </summary>
        private static readonly string k_SettingsPath = Path.Combine("UnityTestTools", "Common");
        /// <summary>   Pathname of the settings folder. </summary>
        const string k_SettingsFolder = "Settings";

        /// <summary>   Saves this object. </summary>
        ///
     

        public virtual void Save()
        {
            EditorUtility.SetDirty(this);
        }

        /// <summary>   Gets the load. </summary>
        ///
     
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        ///
        /// <returns>   A T. </returns>

        public static T Load<T>() where T : ProjectSettingsBase, new ()
        {
            var pathsInProject = Directory.GetDirectories("Assets", "*", SearchOption.AllDirectories)
                                 .Where(s => s.Contains(k_SettingsPath));

            if (pathsInProject.Count() == 0) Debug.LogError("Can't find settings path: " + k_SettingsPath);

            string pathInProject = Path.Combine(pathsInProject.First(), k_SettingsFolder);
            var assetPath = Path.Combine(pathInProject, typeof(T).Name) + ".asset";
            var settings = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;

            if (settings != null) return settings;

            settings = CreateInstance<T>();
            Directory.CreateDirectory(pathInProject);
            AssetDatabase.CreateAsset(settings, assetPath);
            return settings;
        }
    }
}
