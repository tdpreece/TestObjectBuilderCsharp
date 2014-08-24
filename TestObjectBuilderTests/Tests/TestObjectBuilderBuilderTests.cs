using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

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

            [Test]
            public void ProductBuilderHasTwoProperitesWhenProductHasTwoProperties()
            {
                // Arrange
                // Act
                ITestObjBuilder<ProductWithTwoPublicReadWriteProperties> builder =
    TestObjectBuilderBuilder<ProductWithTwoPublicReadWriteProperties>.CreateNewObject();
                // Assert
                Assert.AreEqual(2, builder.GetType().GetProperties().Count());
            }

            [Test]
            public void ProductBuilderHasSamePublicReadWriteProperitesAsProduct()
            {
                // Arrange
                // Act
                ITestObjBuilder<ProductWithTwoPublicReadWriteProperties> builder =
    TestObjectBuilderBuilder<ProductWithTwoPublicReadWriteProperties>.CreateNewObject();

                // Assert
                PropertyInfo[] propertyInfos;
                propertyInfos = builder.GetType().GetProperties();
                List<Tuple<string, Type>> builderProperties = new List<Tuple<string, Type>>();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    builderProperties.Add(Tuple.Create<string, Type>(propertyInfo.Name, propertyInfo.PropertyType));
                }
                Assert.Contains(Tuple.Create<string, Type>("FirstDependency", typeof(IDependency1)), builderProperties);
                Assert.Contains(Tuple.Create<string, Type>("SecondDependency", typeof(IDependency2)), builderProperties);
            }
        }
    }
}
