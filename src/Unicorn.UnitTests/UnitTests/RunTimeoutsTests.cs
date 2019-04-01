﻿using System;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class RunTimeoutsTests : NUnitTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Test timeout")]
        public void TestTimeoutsTestTimeout()
        {
            Config.SetSuiteTags("timeouts");
            Config.TestTimeout = TimeSpan.FromSeconds(1);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();

            Assert.That(runner.Outcome.SuitesOutcomes[0].FailedTests, Is.EqualTo(1));
        }
    }
}
