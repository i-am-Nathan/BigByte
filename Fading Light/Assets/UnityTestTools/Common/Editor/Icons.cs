// file:	Assets\UnityTestTools\Common\Editor\Icons.cs
//
// summary:	Implements the icons class

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   An icons. </summary>
    ///
 

    public static class Icons
    {
        /// <summary>   Pathname of the icons folder. </summary>
        const string k_IconsFolderName = "icons";
        /// <summary>   Full pathname of the icons folder file. </summary>
        private static readonly string k_IconsFolderPath = String.Format("UnityTestTools{0}Common{0}Editor{0}{1}", Path.DirectorySeparatorChar, k_IconsFolderName);

        /// <summary>   Full pathname of the icons assets file. </summary>
        private static readonly string k_IconsAssetsPath = "";

        /// <summary>   The fail image. </summary>
        public static readonly Texture2D FailImg;
        /// <summary>   The ignore image. </summary>
        public static readonly Texture2D IgnoreImg;
        /// <summary>   The success image. </summary>
        public static readonly Texture2D SuccessImg;
        /// <summary>   The unknown image. </summary>
        public static readonly Texture2D UnknownImg;
        /// <summary>   The inconclusive image. </summary>
        public static readonly Texture2D InconclusiveImg;
        /// <summary>   The stopwatch image. </summary>
        public static readonly Texture2D StopwatchImg;

        /// <summary>   The graphical user interface unknown image. </summary>
        public static readonly GUIContent GUIUnknownImg;
        /// <summary>   The graphical user interface inconclusive image. </summary>
        public static readonly GUIContent GUIInconclusiveImg;
        /// <summary>   The graphical user interface ignore image. </summary>
        public static readonly GUIContent GUIIgnoreImg;
        /// <summary>   The graphical user interface success image. </summary>
        public static readonly GUIContent GUISuccessImg;
        /// <summary>   The graphical user interface fail image. </summary>
        public static readonly GUIContent GUIFailImg;

        /// <summary>   Static constructor. </summary>
        ///
     

        static Icons()
        {
            var dirs = Directory.GetDirectories("Assets", k_IconsFolderName, SearchOption.AllDirectories).Where(s => s.EndsWith(k_IconsFolderPath));
            if (dirs.Any())
                k_IconsAssetsPath = dirs.First();
            else
                Debug.LogWarning("The UnityTestTools asset folder path is incorrect. If you relocated the tools please change the path accordingly (Icons.cs).");

            FailImg = LoadTexture("failed.png");
            IgnoreImg = LoadTexture("ignored.png");
            SuccessImg = LoadTexture("passed.png");
            UnknownImg = LoadTexture("normal.png");
            InconclusiveImg = LoadTexture("inconclusive.png");
            StopwatchImg = LoadTexture("stopwatch.png");

            GUIUnknownImg = new GUIContent(UnknownImg);
            GUIInconclusiveImg = new GUIContent(InconclusiveImg);
            GUIIgnoreImg = new GUIContent(IgnoreImg);
            GUISuccessImg = new GUIContent(SuccessImg);
            GUIFailImg = new GUIContent(FailImg);
        }

        /// <summary>   Loads a texture. </summary>
        ///
     
        ///
        /// <param name="fileName"> Filename of the file. </param>
        ///
        /// <returns>   The texture. </returns>

        private static Texture2D LoadTexture(string fileName)
        {
            return (Texture2D)AssetDatabase.LoadAssetAtPath(k_IconsAssetsPath + Path.DirectorySeparatorChar + fileName, typeof(Texture2D));
        }
    }
}
