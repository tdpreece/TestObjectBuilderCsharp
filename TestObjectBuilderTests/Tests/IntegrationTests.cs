using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using TestObjectBuilder;

namespace TestObjectBuilderTests.Tests
{
    [TestFixture]
    class IntegrationTests
    {
        [Test]
        public void BuildProductUsingSingleArgumentConstructorAndSettingOneAdditionalProperty()
        {
            // Arrange
            TestObjectConstructorArgument constructorArg = 
                new TestObjectConstructorArgument("ConstructorArg1", typeof(IDependency1));
            TestObjectConstructorArgumentList constructorArguments =
                new TestObjectConstructorArgumentList() { constructorArg };
            Dependency1 inputDependency1Value = new Dependency1();
            Dependency2 inputDependency2Value = new Dependency2();

            // Act
            ITestObjectBuilder<Product> builder = TestObjectBuilderBuilder<Product>.CreateNewObject(
                constructorArguments);
            builder.With(ConstructorArg1 => inputDependency1Value);
            builder.With(SecondDependency => inputDependency2Value);
            Product product = builder.Build();

            // Assert
            Assert.AreEqual(inputDependency1Value, product.FirstDependency);
            Assert.AreEqual(inputDependency2Value, product.SecondDependency);
            Assert.AreEqual(null, product.ThirdDependency);
        }
    }
}
