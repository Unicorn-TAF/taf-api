﻿using NUnit.Framework;
using ProjectSpecific.BO;
using Unicorn.Core.Testing.Verification;
using static Unicorn.Core.Testing.Verification.Matchers.Is;

namespace Tests.UnitTests
{
    [TestFixture]
    public class AssertionsTest
    {
        [Test, Author("Vitaliy Dobriyan")]
        public void IsNullMatcherPositiveTest()
        {
            NUnit.Framework.Assert.Throws<AssertionError>(delegate 
            {
                SoftAssertion assert = new SoftAssertion();
                assert.AssertThat("asd", EqualTo("asd"))
                    .AssertThat(2, EqualTo(2))
                    .AssertThat(new SampleObject(), EqualTo(new SampleObject("ds", 234)))
                    .AssertThat(new SampleObject(), EqualTo(new SampleObject()))
                    .AssertThat("bla-bla-bla message", new SampleObject(), EqualTo(23));

                assert.AssertAll();
            });
        }
    }
}
