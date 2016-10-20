// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\IntegrationTest.cs
//
// summary:	Implements the integration test class

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>   An integration test. </summary>
///
/// <remarks>    . </remarks>

public static class IntegrationTest
{
    /// <summary>   Message describing the pass. </summary>
    public const string passMessage = "IntegrationTest Pass";
    /// <summary>   Message describing the fail. </summary>
    public const string failMessage = "IntegrationTest Fail";

    /// <summary>   Pass the given go. </summary>
    ///
 

    public static void Pass()
    {
        LogResult(passMessage);
    }

    /// <summary>   Pass the given go. </summary>
    ///
 
    ///
    /// <param name="go">   The go. </param>

    public static void Pass(GameObject go)
    {
        LogResult(go, passMessage);
    }

    /// <summary>   Fails the given go. </summary>
    ///
 
    ///
    /// <param name="reason">   The reason. </param>

    public static void Fail(string reason)
    {
        Fail();
        if (!string.IsNullOrEmpty(reason)) Debug.Log(reason);
    }

    /// <summary>   Fails the given go. </summary>
    ///
 
    ///
    /// <param name="go">       The go. </param>
    /// <param name="reason">   The reason. </param>

    public static void Fail(GameObject go, string reason)
    {
        Fail(go);
        if (!string.IsNullOrEmpty(reason)) Debug.Log(reason);
    }

    /// <summary>   Fails the given go. </summary>
    ///
 

    public static void Fail()
    {
        LogResult(failMessage);
    }

    /// <summary>   Fails the given go. </summary>
    ///
 
    ///
    /// <param name="go">   The go. </param>

    public static void Fail(GameObject go)
    {
        LogResult(go, failMessage);
    }

    /// <summary>   Asserts. </summary>
    ///
 
    ///
    /// <param name="condition">    True to condition. </param>

    public static void Assert(bool condition)
    {
        Assert(condition, "");
    }

    /// <summary>   Asserts. </summary>
    ///
 
    ///
    /// <param name="go">           The go. </param>
    /// <param name="condition">    True to condition. </param>

    public static void Assert(GameObject go, bool condition)
    {
        Assert(go, condition, "");
    }

    /// <summary>   Asserts. </summary>
    ///
 
    ///
    /// <param name="condition">    True to condition. </param>
    /// <param name="message">      The message. </param>

    public static void Assert(bool condition, string message)
    {
        if (!condition) 
            Fail(message);
    }

    /// <summary>   Asserts. </summary>
    ///
 
    ///
    /// <param name="go">           The go. </param>
    /// <param name="condition">    True to condition. </param>
    /// <param name="message">      The message. </param>

    public static void Assert(GameObject go, bool condition, string message)
    {
        if (!condition) 
            Fail(go, message);
    }

    /// <summary>   Logs a result. </summary>
    ///
 
    ///
    /// <param name="message">  The message. </param>

    private static void LogResult(string message)
    {
        Debug.Log(message);
    }

    /// <summary>   Logs a result. </summary>
    ///
 
    ///
    /// <param name="go">       The go. </param>
    /// <param name="message">  The message. </param>

    private static void LogResult(GameObject go, string message)
    {
        Debug.Log(message + " (" + FindTestObject(go).name + ")", go);
    }

    /// <summary>   Searches for the first test object. </summary>
    ///
 
    ///
    /// <param name="go">   The go. </param>
    ///
    /// <returns>   The found test object. </returns>

    private static GameObject FindTestObject(GameObject go)
    {
        var temp = go;
        while (temp.transform.parent != null)
        {
            if (temp.GetComponent("TestComponent") != null)
                return temp;
            temp = temp.transform.parent.gameObject;
        }
        return go;
    }

    #region Dynamic test attributes

    /// <summary>   Attribute for exclude platform. </summary>
    ///
 

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExcludePlatformAttribute : Attribute
    {
        /// <summary>   The platforms to exclude. </summary>
        public string[] platformsToExclude;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="platformsToExclude">   A variable-length parameters list containing platforms to
        ///                                     exclude. </param>

        public ExcludePlatformAttribute(params RuntimePlatform[] platformsToExclude)
        {
            this.platformsToExclude = platformsToExclude.Select(platform => platform.ToString()).ToArray();
        }
    }

    /// <summary>   An expect exceptions. </summary>
    ///
 

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExpectExceptions : Attribute
    {
        /// <summary>   List of names of the exception types. </summary>
        public string[] exceptionTypeNames;
        /// <summary>   True to succeed on exception. </summary>
        public bool succeedOnException;

        /// <summary>   Default constructor. </summary>
        ///
     

        public ExpectExceptions() : this(false)
        {
        }

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="succeedOnException">   True to succeed on exception. </param>

        public ExpectExceptions(bool succeedOnException) : this(succeedOnException, new string[0])
        {
        }

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="succeedOnException">   True to succeed on exception. </param>
        /// <param name="exceptionTypeNames">   A variable-length parameters list containing exception
        ///                                                                      type names. </param>

        public ExpectExceptions(bool succeedOnException, params string[] exceptionTypeNames)
        {
            this.succeedOnException = succeedOnException;
            this.exceptionTypeNames = exceptionTypeNames;
        }

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="succeedOnException">   True to succeed on exception. </param>
        /// <param name="exceptionTypes">       A variable-length parameters list containing exception
        ///                                     types. </param>

        public ExpectExceptions(bool succeedOnException, params Type[] exceptionTypes)
            : this(succeedOnException, exceptionTypes.Select(type => type.FullName).ToArray())
        {
        }

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="exceptionTypeNames">   A variable-length parameters list containing exception
        ///                                     type names. </param>

        public ExpectExceptions(params string[] exceptionTypeNames) : this(false, exceptionTypeNames)
        {
        }

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="exceptionTypes">   A variable-length parameters list containing exception types. </param>

        public ExpectExceptions(params Type[] exceptionTypes) : this(false, exceptionTypes)
        {
        }
    }

    /// <summary>   Attribute for ignore. </summary>
    ///
 

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute
    {
    }

    /// <summary>   Attribute for dynamic test. </summary>
    ///
 

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DynamicTestAttribute : Attribute
    {
        /// <summary>   Name of the scene. </summary>
        private readonly string m_SceneName;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="sceneName">    Name of the scene. </param>

        public DynamicTestAttribute(string sceneName)
        {
            if (sceneName.EndsWith(".unity"))
                sceneName = sceneName.Substring(0, sceneName.Length - ".unity".Length);
            m_SceneName = sceneName;
        }

        /// <summary>   Include on scene. </summary>
        ///
     
        ///
        /// <param name="sceneName">    Name of the scene. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool IncludeOnScene(string sceneName)
        {
            var fileName = Path.GetFileNameWithoutExtension(sceneName);
            return fileName == m_SceneName;
        }
    }

    /// <summary>   A succeed with assertions. </summary>
    ///
 

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SucceedWithAssertions : Attribute
    {
    }

    /// <summary>   Attribute for timeout. </summary>
    ///
 

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TimeoutAttribute : Attribute
    {
        /// <summary>   The timeout. </summary>
        public float timeout;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="seconds">  The seconds. </param>

        public TimeoutAttribute(float seconds)
        {
            timeout = seconds;
        }
    }

    #endregion
}
