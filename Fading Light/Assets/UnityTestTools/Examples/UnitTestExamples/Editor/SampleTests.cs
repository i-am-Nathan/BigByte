// file:	Assets\UnityTestTools\Examples\UnitTestExamples\Editor\SampleTests.cs
//
// summary:	Implements the sample tests class

using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   (Unit Test Fixture) a sample tests. </summary>
    ///
 

    [TestFixture]
    [Category("Sample Tests")]
    internal class SampleTests
    {
        /// <summary>   (Unit Test Method) tests exception. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        [Test]
        [Category("Failing Tests")]
        public void ExceptionTest()
        {
            throw new Exception("Exception throwing test");
        }

        /// <summary>   (Unit Test Method) tests ignored. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        [Test]
        [Ignore("Ignored test")]
        public void IgnoredTest()
        {
            throw new Exception("Ignored this test");
        }

        /// <summary>   (Unit Test Method) tests slow. </summary>
        ///
     

        [Test]
        [MaxTime(100)]
        [Category("Failing Tests")]
        public void SlowTest()
        {
            Thread.Sleep(200);
        }

        /// <summary>   (Unit Test Method) tests failing. </summary>
        ///
     

        [Test]
        [Category("Failing Tests")]
        public void FailingTest()
        {
            Assert.Fail();
        }

        /// <summary>   (Unit Test Method) tests inconclusive. </summary>
        ///
     

        [Test]
        [Category("Failing Tests")]
        public void InconclusiveTest()
        {
            Assert.Inconclusive();
        }

        /// <summary>   (Unit Test Method) tests passing. </summary>
        ///
     

        [Test]
        public void PassingTest()
        {
            Assert.Pass();
        }

        /// <summary>   (Unit Test Method) tests parameterized. </summary>
        ///
     
        ///
        /// <param name="a">    The int to process. </param>

        [Test]
        public void ParameterizedTest([Values(1, 2, 3)] int a)
        {
            Assert.Pass();
        }

        /// <summary>   (Unit Test Method) tests range. </summary>
        ///
     
        ///
        /// <param name="x">    The maximum. </param>

        [Test]
        public void RangeTest([NUnit.Framework.Range(1, 10, 3)] int x)
        {
            Assert.Pass();
        }

        /// <summary>   (Unit Test Method) tests culture specific. </summary>
        ///
     

        [Test]
        [Culture("pl-PL")]
        public void CultureSpecificTest()
        {
        }

        /// <summary>   (Unit Test Method) tests expected exception. </summary>
        ///
     
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "expected message")]
        public void ExpectedExceptionTest()
        {
            throw new ArgumentException("expected message");
        }

        /// <summary>   The zero. </summary>
        [Datapoint]
        public double zero = 0;
        /// <summary>   The positive. </summary>
        [Datapoint]
        public double positive = 1;
        /// <summary>   The negative. </summary>
        [Datapoint]
        public double negative = -1;
        /// <summary>   The maximum. </summary>
        [Datapoint]
        public double max = double.MaxValue;
        /// <summary>   The infinity. </summary>
        [Datapoint]
        public double infinity = double.PositiveInfinity;

        /// <summary>   Square root definition. </summary>
        ///
     
        ///
        /// <param name="num">  Number of. </param>

        [Theory]
        public void SquareRootDefinition(double num)
        {
            Assume.That(num >= 0.0 && num < double.MaxValue);

            var sqrt = Math.Sqrt(num);

            Assert.That(sqrt >= 0.0);
            Assert.That(sqrt * sqrt, Is.EqualTo(num).Within(0.000001));
        }
    }
}
