// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\PlatformRunner\PlayerSettingConfigurator.cs
//
// summary:	Implements the player setting configurator class

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A player setting configurator. </summary>
    ///
 

    class PlayerSettingConfigurator
    {
        /// <summary>   Gets the full pathname of the resources file. </summary>
        ///
        /// <value> The full pathname of the resources file. </value>

        private string resourcesPath {
            get { return m_Temp ? k_TempPath : m_ProjectResourcesPath; }
        }

        /// <summary>   Full pathname of the project resources file. </summary>
        private readonly string m_ProjectResourcesPath = Path.Combine("Assets", "Resources");
        /// <summary>   Full pathname of the temporary file. </summary>
        const string k_TempPath = "Temp";
        /// <summary>   True to temporary. </summary>
        private readonly bool m_Temp;

        /// <summary>   The display resolution dialog. </summary>
        private ResolutionDialogSetting m_DisplayResolutionDialog;
        /// <summary>   True to run in background. </summary>
        private bool m_RunInBackground;
        /// <summary>   True to full screen. </summary>
        private bool m_FullScreen;
        /// <summary>   True to resizable window. </summary>
        private bool m_ResizableWindow;
        /// <summary>   List of temporary files. </summary>
        private readonly List<string> m_TempFileList = new List<string>();

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="saveInTempFolder"> True to save in temporary folder. </param>

        public PlayerSettingConfigurator(bool saveInTempFolder)
        {
            m_Temp = saveInTempFolder;
        }

        /// <summary>   Change settings for integration tests. </summary>
        ///
     

        public void ChangeSettingsForIntegrationTests()
        {
            m_DisplayResolutionDialog = PlayerSettings.displayResolutionDialog;
            PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;

            m_RunInBackground = PlayerSettings.runInBackground;
            PlayerSettings.runInBackground = true;

            m_FullScreen = PlayerSettings.defaultIsFullScreen;
            PlayerSettings.defaultIsFullScreen = false;

            m_ResizableWindow = PlayerSettings.resizableWindow;
            PlayerSettings.resizableWindow = true;
        }

        /// <summary>   Revert settings changes. </summary>
        ///
     

        public void RevertSettingsChanges()
        {
            PlayerSettings.defaultIsFullScreen = m_FullScreen;
            PlayerSettings.runInBackground = m_RunInBackground;
            PlayerSettings.displayResolutionDialog = m_DisplayResolutionDialog;
            PlayerSettings.resizableWindow = m_ResizableWindow;
        }

        /// <summary>   Adds a configuration file to 'content'. </summary>
        ///
     
        ///
        /// <param name="fileName"> Filename of the file. </param>
        /// <param name="content">  The content. </param>

        public void AddConfigurationFile(string fileName, string content)
        {
            var resourcesPathExists = Directory.Exists(resourcesPath);
            if (!resourcesPathExists) AssetDatabase.CreateFolder("Assets", "Resources");

            var filePath = Path.Combine(resourcesPath, fileName);
            File.WriteAllText(filePath, content);

            m_TempFileList.Add(filePath);
        }

        /// <summary>   Removes all configuration files. </summary>
        ///
     

        public void RemoveAllConfigurationFiles()
        {
            foreach (var filePath in m_TempFileList)
                AssetDatabase.DeleteAsset(filePath);
            if (Directory.Exists(resourcesPath)
                && Directory.GetFiles(resourcesPath).Length == 0)
                AssetDatabase.DeleteAsset(resourcesPath);
        }
    }
}
