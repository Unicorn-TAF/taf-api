﻿using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Parameterized test suite"), Parameterized]
    [Tag("parameterizedBroken")]
    public class UParameterizedSuiteNoSuiteData : TestSuite
    {
        public UParameterizedSuiteNoSuiteData(SampleObject so)
        {
        }

        public static string Output { get; set; }

        [Test("Test 1")]
        public void Test1() =>
            Output += "Test1>";
    }
}