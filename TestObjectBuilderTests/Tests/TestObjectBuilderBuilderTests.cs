using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using TestObjectBuilder;

namespace TestObjectBuilderTests.Tests
{
    public class TestObjectBuilderBuilderTests
    {
        [TestFixture]
        public class CreateNewObject
        {
            [Test]
            public void CreatesProductWithoutPropertiesAndAZeroArgConstructor()
            {
                // Arrange
                ITestObjBuilder<ProductWithoutProperties> builder = 
                    TestObjectBuilderBuilder<ProductWithoutProperties>.CreateNewObject();

                // Act
                ProductWithoutProperties product = builder.Build();

                // Assert
                Assert.NotNull(product);
            }

        }
    }
}
