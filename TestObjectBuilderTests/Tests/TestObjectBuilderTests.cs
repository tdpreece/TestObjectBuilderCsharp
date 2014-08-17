using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using TestObjectBuilder;

namespace TestObjectBuilderTests
{
    /**
     * Tests to do:
     * done - Add a SimpleProduct class
     * done - Add a SimpleProductBuilder class (Needs: ctor, Build and properties).
     * - with()
     * - but()
     * - build()
     * - product is simple class
     * - product uses inheritance
     * 
     * 
     * - Add a TestBuilderFactory class.  Don't want to do this in the tests because
     *   want to show class under test explicitly in the tests, thus, it may be an
     *   idea to add an examples project to show how that TestObject Builder could be used.
     *   
     * - Add functionality to automatically generate a TestObjectBuilder from a class using reflection.
     *   calls a default ctor, then sets all properties (public, private and protected) after construction.
     * 
     */
    public class TestObjectBuilderTests
    {
        [TestFixture]
        public class Build
        {
            [Test]
            public void BuildingWithoutSpecifyingDependency1ExternallyUsesDummyDependency1()
            {
                // Arrange
                ITestObjBuilder<Product> productBuilder = new ProductTestObjectBuilder();

                // Act
                Product product = productBuilder.Build();

                // Assert
                Assert.IsInstanceOf(typeof(DummyDependency1), product.FirstDependency);
            }
        }

        [TestFixture]
        public class With
        {
            [Test]
            public void BuildWithExternallySpecifiedDependency1ResultsInProductUsingExternallySpecifiedDependency1()
            {
                // Arrange
                DummyDependency1 externallySuppliedDependency1 = new DummyDependency1();
                ITestObjBuilder<Product> productBuilder = new ProductTestObjectBuilder();

                // Act
                productBuilder = productBuilder.With(FirstDependency => externallySuppliedDependency1);
                Product product = productBuilder.Build();

                // Assert
                Assert.AreEqual(externallySuppliedDependency1, product.FirstDependency);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ExceptionIsRaisedWhenTryToSpecifyExternalDependencyForPropertyThatDoesNotExist()
            {
                // Arrange
                DummyDependency1 externallySuppliedDependency1 = new DummyDependency1();
                ITestObjBuilder<Product> productBuilder = new ProductTestObjectBuilder();

                // Act
                productBuilder = productBuilder.With(PropertyNameThatDoesNotExist => externallySuppliedDependency1);
            }
        }
    }
}
