// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\TestComponent.cs
//
// summary:	Implements the test component class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityTest
{
    /// <summary>   Interface for test component. </summary>
    ///
 

    public interface ITestComponent : IComparable<ITestComponent>
    {
        /// <summary>   Tests enable. </summary>
        ///
        /// <param name="enable">   True to enable, false to disable. </param>

        void EnableTest(bool enable);

        /// <summary>   Query if this object is test group. </summary>
        ///
        /// <returns>   True if test group, false if not. </returns>

        bool IsTestGroup();

        /// <summary>   Gets the game object. </summary>
        ///
        /// <value> The game object. </value>

        GameObject gameObject { get; }

        /// <summary>   Gets the name. </summary>
        ///
        /// <value> The name. </value>

        string Name { get; }

        /// <summary>   Gets test group. </summary>
        ///
        /// <returns>   The test group. </returns>

        ITestComponent GetTestGroup();

        /// <summary>   Query if 'exceptionType' is exception expected. </summary>
        ///
        /// <param name="exceptionType">    Type of the exception. </param>
        ///
        /// <returns>   True if exception expected, false if not. </returns>

        bool IsExceptionExpected(string exceptionType);

        /// <summary>   Determine if we should succeed on exception. </summary>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        bool ShouldSucceedOnException();

        /// <summary>   Gets the timeout. </summary>
        ///
        /// <returns>   The timeout. </returns>

        double GetTimeout();

        /// <summary>   Query if this object is ignored. </summary>
        ///
        /// <returns>   True if ignored, false if not. </returns>

        bool IsIgnored();

        /// <summary>   Determine if we should succeed on assertions. </summary>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        bool ShouldSucceedOnAssertions();

        /// <summary>   Query if this object is exluded on this platform. </summary>
        ///
        /// <returns>   True if exluded on this platform, false if not. </returns>

        bool IsExludedOnThisPlatform();
    }

    /// <summary>   A test component. </summary>
    ///
 

    public class TestComponent : MonoBehaviour, ITestComponent
    {
        /// <summary>   The null test component. </summary>
        public static ITestComponent NullTestComponent = new NullTestComponentImpl();

        /// <summary>   The timeout. </summary>
        public float timeout = 5;
        /// <summary>   True if ignored. </summary>
        public bool ignored = false;
        /// <summary>   True if succeed after all assertions are executed. </summary>
        public bool succeedAfterAllAssertionsAreExecuted = false;
        /// <summary>   True to expect exception. </summary>
        public bool expectException = false;
        /// <summary>   List of expected exceptions. </summary>
        public string expectedExceptionList = "";
        /// <summary>   True if succeed when exception is thrown. </summary>
        public bool succeedWhenExceptionIsThrown = false;
        /// <summary>   The included platforms. </summary>
        public IncludedPlatforms includedPlatforms = (IncludedPlatforms) ~0L;
        /// <summary>   The platforms to ignore. </summary>
        public string[] platformsToIgnore = null;

        /// <summary>   True to dynamic. </summary>
        public bool dynamic;
        /// <summary>   Name of the dynamic type. </summary>
        public string dynamicTypeName;

        /// <summary>   Query if this object is exluded on this platform. </summary>
        ///
     
        ///
        /// <returns>   True if exluded on this platform, false if not. </returns>

        public bool IsExludedOnThisPlatform()
        {
            return platformsToIgnore != null && platformsToIgnore.Any(platform => platform == Application.platform.ToString());
        }

        /// <summary>   Query if 'a' is assignable from. </summary>
        ///
     
        ///
        /// <param name="a">    The TestComponent to process. </param>
        /// <param name="b">    The TestComponent to process. </param>
        ///
        /// <returns>   True if assignable from, false if not. </returns>

        static bool IsAssignableFrom(Type a, Type b)
        {
#if !UNITY_METRO
            return a.IsAssignableFrom(b);
#else
            return false;
#endif
        }

        /// <summary>   Query if 'exception' is exception expected. </summary>
        ///
     
        ///
        /// <param name="exception">    The exception. </param>
        ///
        /// <returns>   True if exception expected, false if not. </returns>

        public bool IsExceptionExpected(string exception)
		{
			exception = exception.Trim();
            if (!expectException) 
				return false;
			if(string.IsNullOrEmpty(expectedExceptionList.Trim())) 
				return true;
            foreach (var expectedException in expectedExceptionList.Split(',').Select(e => e.Trim()))
            {
                if (exception == expectedException) 
					return true;
                var exceptionType = Type.GetType(exception) ?? GetTypeByName(exception);
                var expectedExceptionType = Type.GetType(expectedException) ?? GetTypeByName(expectedException);
                if (exceptionType != null && expectedExceptionType != null && IsAssignableFrom(expectedExceptionType, exceptionType))
                    return true;
            }
            return false;
        }

        /// <summary>   Determine if we should succeed on exception. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool ShouldSucceedOnException()
        {
            return succeedWhenExceptionIsThrown;
        }

        /// <summary>   Gets the timeout. </summary>
        ///
     
        ///
        /// <returns>   The timeout. </returns>

        public double GetTimeout()
        {
            return timeout;
        }

        /// <summary>   Query if this object is ignored. </summary>
        ///
     
        ///
        /// <returns>   True if ignored, false if not. </returns>

        public bool IsIgnored()
        {
            return ignored;
        }

        /// <summary>   Determine if we should succeed on assertions. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool ShouldSucceedOnAssertions()
        {
            return succeedAfterAllAssertionsAreExecuted;
        }

        /// <summary>   Gets type by name. </summary>
        ///
     
        ///
        /// <param name="className">    Name of the class. </param>
        ///
        /// <returns>   The type by name. </returns>

        private static Type GetTypeByName(string className)
        {
#if !UNITY_METRO
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(type => type.Name == className);
#else
            return null;
#endif
        }

        /// <summary>   Executes the validate action. </summary>
        ///
     

        public void OnValidate()
        {
            if (timeout < 0.01f) timeout = 0.01f;
        }

        // Legacy

        /// <summary>   A bit-field of flags for specifying included platforms. </summary>
        ///
     

        [Flags]
        public enum IncludedPlatforms
        {
            /// <summary>   A binary constant representing the windows editor flag. </summary>
            WindowsEditor       = 1 << 0,
                /// <summary>   A binary constant representing the osx editor flag. </summary>
                OSXEditor           = 1 << 1,
                /// <summary>   A binary constant representing the windows player flag. </summary>
                WindowsPlayer       = 1 << 2,
                /// <summary>   A binary constant representing the osx player flag. </summary>
                OSXPlayer           = 1 << 3,
                /// <summary>   A binary constant representing the linux player flag. </summary>
                LinuxPlayer         = 1 << 4,
                /// <summary>   A binary constant representing the metro player X coordinate 86 flag. </summary>
                MetroPlayerX86      = 1 << 5,
                /// <summary>   A binary constant representing the metro player X coordinate 64 flag. </summary>
                MetroPlayerX64      = 1 << 6,
                /// <summary>   A binary constant representing the metro player a Remove flag. </summary>
                MetroPlayerARM      = 1 << 7,
                /// <summary>   A binary constant representing the windows web player flag. </summary>
                WindowsWebPlayer    = 1 << 8,
                /// <summary>   A binary constant representing the osx web player flag. </summary>
                OSXWebPlayer        = 1 << 9,
                /// <summary>   A binary constant representing the android flag. </summary>
                Android             = 1 << 10,
// ReSharper disable once InconsistentNaming
                /// <summary>   A binary constant representing the iPhone player flag. </summary>
                IPhonePlayer        = 1 << 11,
                /// <summary>   A binary constant representing the tizen player flag. </summary>
                TizenPlayer         = 1 << 12,
                /// <summary>   A binary constant representing the wp 8 player flag. </summary>
                WP8Player           = 1 << 13,
                /// <summary>   A binary constant representing the bb 10 player flag. </summary>
                BB10Player          = 1 << 14,
                /// <summary>   A binary constant representing the na cl flag. </summary>
                NaCl                = 1 << 15,
                /// <summary>   A binary constant representing the ps 3 flag. </summary>
                PS3                 = 1 << 16,
                /// <summary>   A binary constant representing the xbox 360 flag. </summary>
                XBOX360             = 1 << 17,
                /// <summary>   A binary constant representing the wii player flag. </summary>
                WiiPlayer           = 1 << 18,
                /// <summary>   A binary constant representing the psp 2 flag. </summary>
                PSP2                = 1 << 19,
                /// <summary>   A binary constant representing the ps 4 flag. </summary>
                PS4                 = 1 << 20,
                /// <summary>   A binary constant representing the psm player flag. </summary>
                PSMPlayer           = 1 << 21,
                /// <summary>   A binary constant representing the xbox one flag. </summary>
                XboxOne             = 1 << 22,
        }

        #region ITestComponent implementation

        /// <summary>   Tests enable. </summary>
        ///
     
        ///
        /// <param name="enable">   True to enable, false to disable. </param>

        public void EnableTest(bool enable)
        {
            if (enable && dynamic)
            {
                Type t = Type.GetType(dynamicTypeName);
                var s = gameObject.GetComponent(t) as MonoBehaviour;
                if (s != null)
                    DestroyImmediate(s);

                gameObject.AddComponent(t);
            }

            if (gameObject.activeSelf != enable) gameObject.SetActive(enable);
        }

        /// <summary>
        /// Compares this ITestComponent object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="obj">  I test component to compare to this. </param>
        ///
        /// <returns>
        /// Negative if this object is less than the other, 0 if they are equal, or positive if this is
        /// greater.
        /// </returns>

        public int CompareTo(ITestComponent obj)
        {
            if (obj == NullTestComponent)
                return 1;
            var result = gameObject.name.CompareTo(obj.gameObject.name);
            if (result == 0)
                result = gameObject.GetInstanceID().CompareTo(obj.gameObject.GetInstanceID());
            return result;
        }

        /// <summary>   Query if this object is test group. </summary>
        ///
     
        ///
        /// <returns>   True if test group, false if not. </returns>

        public bool IsTestGroup()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var childTc = gameObject.transform.GetChild(i).GetComponent(typeof(TestComponent));
                if (childTc != null)
                    return true;
            }
            return false;
        }

        /// <summary>   Gets the name. </summary>
        ///
        /// <value> The name. </value>

        public string Name { get { return gameObject == null ? "" : gameObject.name; } }

        /// <summary>   Gets test group. </summary>
        ///
     
        ///
        /// <returns>   The test group. </returns>

        public ITestComponent GetTestGroup()
        {
            var parent = gameObject.transform.parent;
            if (parent == null)
                return NullTestComponent;
            return parent.GetComponent<TestComponent>();
        }

        /// <summary>   Tests if this object is considered equal to another. </summary>
        ///
     
        ///
        /// <param name="o">    The object to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>

        public override bool Equals(object o)
        {
            if (o is TestComponent)
                return this == (o as TestComponent);
            return false;
        }

        /// <summary>   Calculates a hash code for this object. </summary>
        ///
     
        ///
        /// <returns>   A hash code for this object. </returns>

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>   Equality operator. </summary>
        ///
     
        ///
        /// <param name="a">    The TestComponent to process. </param>
        /// <param name="b">    The TestComponent to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>

        public static bool operator ==(TestComponent a, TestComponent b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (((object)a == null) || ((object)b == null))
                return false;
            if (a.dynamic && b.dynamic)
                return a.dynamicTypeName == b.dynamicTypeName;
            if (a.dynamic || b.dynamic)
                return false;
            return a.gameObject == b.gameObject;
        }

        /// <summary>   Inequality operator. </summary>
        ///
     
        ///
        /// <param name="a">    The TestComponent to process. </param>
        /// <param name="b">    The TestComponent to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>

        public static bool operator !=(TestComponent a, TestComponent b)
        {
            return !(a == b);
        }

        #endregion

        #region Static helpers

        /// <summary>   Tests create dynamic. </summary>
        ///
     
        ///
        /// <param name="type"> The type. </param>
        ///
        /// <returns>   The new dynamic test. </returns>

        public static TestComponent CreateDynamicTest(Type type)
        {
            var go = CreateTest(type.Name);
            go.hideFlags |= HideFlags.DontSave;
            go.SetActive(false);

            var tc = go.GetComponent<TestComponent>();
            tc.dynamic = true;
            tc.dynamicTypeName = type.AssemblyQualifiedName;

#if !UNITY_METRO
            foreach (var typeAttribute in type.GetCustomAttributes(false))
            {
                if (typeAttribute is IntegrationTest.TimeoutAttribute)
                    tc.timeout = (typeAttribute as IntegrationTest.TimeoutAttribute).timeout;
                else if (typeAttribute is IntegrationTest.IgnoreAttribute)
                    tc.ignored = true;
                else if (typeAttribute is IntegrationTest.SucceedWithAssertions)
                    tc.succeedAfterAllAssertionsAreExecuted = true;
                else if (typeAttribute is IntegrationTest.ExcludePlatformAttribute)
                    tc.platformsToIgnore = (typeAttribute as IntegrationTest.ExcludePlatformAttribute).platformsToExclude;
                else if (typeAttribute is IntegrationTest.ExpectExceptions)
                {
                    var attribute = (typeAttribute as IntegrationTest.ExpectExceptions);
                    tc.expectException = true;
                    tc.expectedExceptionList = string.Join(",", attribute.exceptionTypeNames);
                    tc.succeedWhenExceptionIsThrown = attribute.succeedOnException;
                }
            }
            go.AddComponent(type);
#endif  // if !UNITY_METRO
            return tc;
        }

        /// <summary>   Tests create. </summary>
        ///
     
        ///
        /// <returns>   The new test. </returns>

        public static GameObject CreateTest()
        {
            return CreateTest("New Test");
        }

        /// <summary>   Tests create. </summary>
        ///
     
        ///
        /// <param name="name"> The name. </param>
        ///
        /// <returns>   The new test. </returns>

        private static GameObject CreateTest(string name)
        {
            var go = new GameObject(name);
            go.AddComponent<TestComponent>();
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(go, "Created test");
#endif
            return go;
        }

        /// <summary>   Searches for all tests on scene. </summary>
        ///
     
        ///
        /// <returns>   The found all tests on scene. </returns>

        public static List<TestComponent> FindAllTestsOnScene()
        {
            var tests = Resources.FindObjectsOfTypeAll (typeof(TestComponent)).Cast<TestComponent> ();
#if UNITY_EDITOR
            tests = tests.Where( t => {var p = PrefabUtility.GetPrefabType(t); return p != PrefabType.Prefab && p != PrefabType.ModelPrefab;} );

#endif
            return tests.ToList ();
        }

        /// <summary>   Searches for all top tests on scene. </summary>
        ///
     
        ///
        /// <returns>   The found all top tests on scene. </returns>

        public static List<TestComponent> FindAllTopTestsOnScene()
        {
            return FindAllTestsOnScene().Where(component => component.gameObject.transform.parent == null).ToList();
        }

        /// <summary>   Searches for all dynamic tests on scene. </summary>
        ///
     
        ///
        /// <returns>   The found all dynamic tests on scene. </returns>

        public static List<TestComponent> FindAllDynamicTestsOnScene()
        {
            return FindAllTestsOnScene().Where(t => t.dynamic).ToList();
        }

        /// <summary>   Destroys all dynamic tests. </summary>
        ///
     

        public static void DestroyAllDynamicTests()
        {
            foreach (var dynamicTestComponent in FindAllDynamicTestsOnScene())
                DestroyImmediate(dynamicTestComponent.gameObject);
        }

        /// <summary>   Disables all tests. </summary>
        ///
     

        public static void DisableAllTests()
        {
            foreach (var t in FindAllTestsOnScene()) t.EnableTest(false);
        }

        /// <summary>   Determines if we can any tests on scene. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public static bool AnyTestsOnScene()
        {
            return FindAllTestsOnScene().Any();
        }

        /// <summary>   Determines if we can any dynamic test for current scene. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public static bool AnyDynamicTestForCurrentScene()
        {
#if UNITY_EDITOR
            return TestComponent.GetTypesWithHelpAttribute(SceneManager.GetActiveScene().name).Any();
#else
            return TestComponent.GetTypesWithHelpAttribute(SceneManager.GetActiveScene().name).Any();
#endif
        }

        #endregion

        /// <summary>   A null test component implementation. This class cannot be inherited. </summary>
        ///
     

        private sealed class NullTestComponentImpl : ITestComponent
        {
            /// <summary>
            /// Compares this ITestComponent object to another to determine their relative ordering.
            /// </summary>
            ///
         
            ///
            /// <param name="other">    Another instance to compare. </param>
            ///
            /// <returns>
            /// Negative if this object is less than the other, 0 if they are equal, or positive if this is
            /// greater.
            /// </returns>

            public int CompareTo(ITestComponent other)
            {
                if (other == this) return 0;
                return -1;
            }

            /// <summary>   Tests enable. </summary>
            ///
         
            ///
            /// <param name="enable">   True to enable, false to disable. </param>

            public void EnableTest(bool enable)
            {
            }

            /// <summary>   Query if this object is test group. </summary>
            ///
         
            ///
            /// <returns>   True if test group, false if not. </returns>

            public bool IsTestGroup()
            {
                throw new NotImplementedException();
            }

            /// <summary>   Gets or sets the game object. </summary>
            ///
            /// <value> The game object. </value>

            public GameObject gameObject { get; private set; }

            /// <summary>   Gets the name. </summary>
            ///
            /// <value> The name. </value>

            public string Name { get { return ""; } }

            /// <summary>   Gets test group. </summary>
            ///
         
            ///
            /// <returns>   The test group. </returns>

            public ITestComponent GetTestGroup()
            {
                return null;
            }

            /// <summary>   Query if 'exceptionType' is exception expected. </summary>
            ///
         
            ///
            /// <param name="exceptionType">    Type of the exception. </param>
            ///
            /// <returns>   True if exception expected, false if not. </returns>

            public bool IsExceptionExpected(string exceptionType)
            {
                throw new NotImplementedException();
            }

            /// <summary>   Determine if we should succeed on exception. </summary>
            ///
         
            ///
            /// <returns>   True if it succeeds, false if it fails. </returns>

            public bool ShouldSucceedOnException()
            {
                throw new NotImplementedException();
            }

            /// <summary>   Gets the timeout. </summary>
            ///
         
            ///
            /// <returns>   The timeout. </returns>

            public double GetTimeout()
            {
                throw new NotImplementedException();
            }

            /// <summary>   Query if this object is ignored. </summary>
            ///
         
            ///
            /// <returns>   True if ignored, false if not. </returns>

            public bool IsIgnored()
            {
                throw new NotImplementedException();
            }

            /// <summary>   Determine if we should succeed on assertions. </summary>
            ///
         
            ///
            /// <returns>   True if it succeeds, false if it fails. </returns>

            public bool ShouldSucceedOnAssertions()
            {
                throw new NotImplementedException();
            }

            /// <summary>   Query if this object is exluded on this platform. </summary>
            ///
         
            ///
            /// <returns>   True if exluded on this platform, false if not. </returns>

            public bool IsExludedOnThisPlatform()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>   Gets the types with help attributes in this collection. </summary>
        ///
     
        ///
        /// <param name="sceneName">    Name of the scene. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process the types with help attributes in
        /// this collection.
        /// </returns>

        public static IEnumerable<Type> GetTypesWithHelpAttribute(string sceneName)
        {
#if !UNITY_METRO
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = null;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Debug.LogError("Failed to load types from: " + assembly.FullName);
                    foreach (Exception loadEx in ex.LoaderExceptions)
                        Debug.LogException(loadEx);
                }

                if (types == null)
                    continue;

                foreach (Type type in types)
                {
                    var attributes = type.GetCustomAttributes(typeof(IntegrationTest.DynamicTestAttribute), true);
                    if (attributes.Length == 1)
                    {
                        var a = attributes.Single() as IntegrationTest.DynamicTestAttribute;
                        if (a.IncludeOnScene(sceneName)) yield return type;
                    }
                }
            }
#else   // if !UNITY_METRO
            yield break;
#endif  // if !UNITY_METRO
        }
    }
}
