using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using TestObjectBuilder;

namespace TestObjectBuilderTests.Tests
{
    public class TestObjectBuilderTests
    {
        [TestFixture]
        public class With
        {
            [Test]
            public void FirstTest()
            {
                Assert.AreEqual(1, 1);
            }            
        }
    }
}
