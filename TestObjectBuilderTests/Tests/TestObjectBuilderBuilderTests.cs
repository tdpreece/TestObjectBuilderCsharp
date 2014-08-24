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
            public void ProductBuilderCreateBuildsObjectsOfTypeProduct()
            {
                // Arrange
                // Act
                ITestObjBuilder<ProductWithoutProperties> builder =
    TestObjectBuilderBuilder<ProductWithoutProperties>.CreateNewObject();
                // Assert
                Assert.AreSame(typeof(ProductWithoutProperties), builder.GetType().GetMethod("Build").ReturnType);
            }

            [Test]
            public void ProductBuilderHasNoPropertiesWhenProductHasNoProperties()
            {
                // Arrange
                // Act
                ITestObjBuilder<ProductWithoutProperties> builder =
                    TestObjectBuilderBuilder<ProductWithoutProperties>.CreateNewObject();

                // Assert
                Assert.AreEqual(0, builder.GetType().GetProperties().Count());
            }
        }
    }
}
