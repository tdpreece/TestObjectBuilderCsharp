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
        public class TestObjectBuilderMethodTest
        {
            [SetUp]
            public void init()
            {
                _productBuilder = new ProductTestObjectBuilder();
            }

            protected ProductTestObjectBuilder _productBuilder;
        }

        [TestFixture]
        public class Build : TestObjectBuilderMethodTest
        {
            [Test]
            public void BuildingWithoutSpecifyingDependency1ExternallyUsesDummyDependency1()
            {
                // Act
                Product product = this._productBuilder.Build();

                // Assert
                Assert.IsInstanceOf(typeof(DummyDependency1), product.FirstDependency);
            }
        }

        [TestFixture]
        public class With : TestObjectBuilderMethodTest
        {
            [Test]
            public void BuildWithExternallySpecifiedDependency1ResultsInProductUsingExternallySpecifiedDependency1()
            {
                // Arrange
                DummyDependency1 externallySuppliedDependency1 = new DummyDependency1();

                // Act
                this._productBuilder.With(FirstDependency => externallySuppliedDependency1);
                Product product = this._productBuilder.Build();

                // Assert
                Assert.AreEqual(externallySuppliedDependency1, product.FirstDependency);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ExceptionIsThrownWhenTryToSpecifyExternalDependencyForPropertyThatDoesNotExist()
            {
                // Arrange
                DummyDependency1 externallySuppliedDependency1 = new DummyDependency1();

                // Act
                this._productBuilder.With(PropertyNameThatDoesNotExist => externallySuppliedDependency1);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ExceptionIsThrownWhenTryToSpecifyExternalDependencyOfWrongType()
            {
                // Arrange
                Dependency2 dependencyOfWrongType = new Dependency2();

                // Act
                this._productBuilder.With(FirstDependency => dependencyOfWrongType);
            }
        }
    }
}
