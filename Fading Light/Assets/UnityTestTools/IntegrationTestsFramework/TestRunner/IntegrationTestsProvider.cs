// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\IntegrationTestsProvider.cs
//
// summary:	Implements the integration tests provider class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTest.IntegrationTestRunner
{
    /// <summary>   An integration tests provider. </summary>
    ///
 

    class IntegrationTestsProvider
    {
        /// <summary>   Collection of tests. </summary>
        internal Dictionary<ITestComponent, HashSet<ITestComponent>> testCollection = new Dictionary<ITestComponent, HashSet<ITestComponent>>();
        /// <summary>   The current test group. </summary>
        internal ITestComponent currentTestGroup;
        /// <summary>   The test to run. </summary>
        internal IEnumerable<ITestComponent> testToRun;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="tests">    The tests. </param>

        public IntegrationTestsProvider(IEnumerable<ITestComponent> tests)
        {
            testToRun = tests;
            foreach (var test in tests.OrderBy(component => component))
            {
                if (test.IsTestGroup())
                {
                    throw new Exception(test.Name + " is test a group");
                }
                AddTestToList(test);
            }
            if (currentTestGroup == null)
            {
                currentTestGroup = FindInnerTestGroup(TestComponent.NullTestComponent);
            }
        }

        /// <summary>   Adds a test to list. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>

        private void AddTestToList(ITestComponent test)
        {
            var group = test.GetTestGroup();
            if (!testCollection.ContainsKey(group))
                testCollection.Add(group, new HashSet<ITestComponent>());
            testCollection[group].Add(test);
            if (group == TestComponent.NullTestComponent) return;
            AddTestToList(group);
        }

        /// <summary>   Gets the next test. </summary>
        ///
     
        ///
        /// <returns>   The next test. </returns>

        public ITestComponent GetNextTest()
        {
            var test = testCollection[currentTestGroup].First();
            testCollection[currentTestGroup].Remove(test);
            test.EnableTest(true);
            return test;
        }

        /// <summary>   Tests finish. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>

        public void FinishTest(ITestComponent test)
        {
            try
            {
                test.EnableTest(false);
                currentTestGroup = FindNextTestGroup(currentTestGroup);
            }
            catch (MissingReferenceException e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>   Searches for the next test group. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="testGroup">    Group the test belongs to. </param>
        ///
        /// <returns>   The found test group. </returns>

        private ITestComponent FindNextTestGroup(ITestComponent testGroup)
        {
            if (testGroup == null) 
                throw new Exception ("No test left");

            if (testCollection[testGroup].Any())
            {
                testGroup.EnableTest(true);
                return FindInnerTestGroup(testGroup);
            }
            testCollection.Remove(testGroup);
            testGroup.EnableTest(false);

            var parentTestGroup = testGroup.GetTestGroup();
            if (parentTestGroup == null) return null;

            testCollection[parentTestGroup].Remove(testGroup);
            return FindNextTestGroup(parentTestGroup);
        }

        /// <summary>   Searches for the first inner test group. </summary>
        ///
     
        ///
        /// <param name="group">    The group. </param>
        ///
        /// <returns>   The found inner test group. </returns>

        private ITestComponent FindInnerTestGroup(ITestComponent group)
        {
            var innerGroups = testCollection[group];
            foreach (var innerGroup in innerGroups)
            {
                if (!innerGroup.IsTestGroup()) continue;
                innerGroup.EnableTest(true);
                return FindInnerTestGroup(innerGroup);
            }
            return group;
        }

        /// <summary>   Determines if we can any tests left. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool AnyTestsLeft()
        {
            return testCollection.Count != 0;
        }

        /// <summary>   Gets remaining tests. </summary>
        ///
     
        ///
        /// <returns>   The remaining tests. </returns>

        public List<ITestComponent> GetRemainingTests()
        {
            var remainingTests = new List<ITestComponent>();
            foreach (var test in testCollection)
            {
                remainingTests.AddRange(test.Value);
            }
            return remainingTests;
        }
    }
}
