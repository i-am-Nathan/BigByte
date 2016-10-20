// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\EditorReferencesUtil.cs
//
// summary:	Implements the editor references utility class

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityTest
{
    /// <summary>   An editor references utility. </summary>
    ///
 

    public static class EditorReferencesUtil
    {
        /// <summary>   Searches for the first scenes which contain asset. </summary>
        ///
     
        ///
        /// <param name="file"> The file. </param>
        ///
        /// <returns>   The found scenes which contain asset. </returns>

        public static List<Object> FindScenesWhichContainAsset(string file)
        {
            string assetPath = GetAssetPathFromFileNameAndExtension (file);
            Object cur = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
            return AllScenes.Where(a => ADependsOnB(a, cur)).ToList();
        }

        /// <summary>   Clean path separators. </summary>
        ///
     
        ///
        /// <param name="s">    The string. </param>
        ///
        /// <returns>   A string. </returns>

        private static string CleanPathSeparators(string s)
        {
            const string forwardSlash = "/";
            const string backSlash = "\\";
            return s.Replace(backSlash, forwardSlash);
        }

        /// <summary>   Gets relative asset path from full path. </summary>
        ///
     
        ///
        /// <param name="fullPath"> Full pathname of the full file. </param>
        ///
        /// <returns>   The relative asset path from full path. </returns>

        private static string GetRelativeAssetPathFromFullPath(string fullPath)
        {
            fullPath = CleanPathSeparators(fullPath);
            if (fullPath.Contains(Application.dataPath))
            {
                return fullPath.Replace(Application.dataPath, "Assets");
            }
            Debug.LogWarning("Path does not point to a location within Assets: " + fullPath);
            return null;
        }

        /// <summary>   Gets asset path from file name and extension. </summary>
        ///
     
        ///
        /// <param name="assetName">    Name of the asset. </param>
        ///
        /// <returns>   The asset path from file name and extension. </returns>

        private static string GetAssetPathFromFileNameAndExtension(string assetName)
        {
            string[] assets = AssetDatabase.FindAssets (Path.GetFileNameWithoutExtension (assetName));
            string assetPath = null;
            
            foreach (string guid in assets) {
                string relativePath = AssetDatabase.GUIDToAssetPath (guid);
                
                if (Path.GetFileName (relativePath) == Path.GetFileName (assetName))
                    assetPath = relativePath;
            }
            
            return assetPath;
        }

        /// <summary>   Dir search. </summary>
        ///
     
        ///
        /// <param name="d">            The DirectoryInfo to process. </param>
        /// <param name="searchFor">    The search for. </param>
        ///
        /// <returns>   A List&lt;FileInfo&gt; </returns>

        private static List<FileInfo> DirSearch(DirectoryInfo d, string searchFor)
        {
            List<FileInfo> founditems = d.GetFiles(searchFor).ToList();
            
            // Add (by recursing) subdirectory items.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
                founditems.AddRange(DirSearch(di, searchFor));
            
            return (founditems);
        }

        /// <summary>   Gets all scenes. </summary>
        ///
        /// <value> all scenes. </value>

        private static List<Object> AllScenes
        {
            get
            {
                // get every single one of the files in the Assets folder.
                List<FileInfo> files = DirSearch(new DirectoryInfo(Application.dataPath), "*.unity");
                
                // now make them all into Asset references.
                List<Object> assetRefs = new List<Object>();
                
                foreach (FileInfo fi in files)
                {
                    if (fi.Name.StartsWith("."))
                        continue;   // Unity ignores dotfiles.
                    assetRefs.Add(AssetDatabase.LoadMainAssetAtPath(GetRelativeAssetPathFromFullPath(fi.FullName)));
                }
                return assetRefs;
            }
        }

        /// <summary>   Depends on b. </summary>
        ///
     
        ///
        /// <param name="obj">          The object. </param>
        /// <param name="selectedObj">  The selected object. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        private static bool ADependsOnB(Object obj, Object selectedObj)
        {
            if (selectedObj == null) return false;
            
            //optionally, exclude self.
            if (selectedObj == obj) return false;
            
            Object[] dependencies = EditorUtility.CollectDependencies(new Object[1] { obj });
            if (dependencies.Length < 2) return false;	 // if there's only one, it's us.
            
            foreach (Object dep in dependencies)
                if (dep == selectedObj)
                    return true;
            return false;
        }
    }
}