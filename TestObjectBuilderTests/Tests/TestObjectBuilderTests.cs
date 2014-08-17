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
     * - add getDependency(dependency name) string.  With returns ITestObjBuilder<T>, which
     *   has no visibility of the dependencies for a concrete class.  Thus you'd have to do,
     *   builder.GetType().GetProperty("FirstDependency").GetValue(builder, null)
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
                IDependency2 dependencyOfWrongType = new DummyDependency2();

                // Act
                this._productBuilder.With(FirstDependency => dependencyOfWrongType);
            }

            [Test]
            public void SetsTwoExternalDependenciesWhenPassedInSingleWithCall()
            {
                // Arrange
                IDependency1 dependency1 = new DummyDependency1();
                IDependency2 dependency2 = new DummyDependency2();

                // Act
                this._productBuilder.With(FirstDependency => dependency1, SecondDependency => dependency2);

                // Assert
                Assert.AreEqual(dependency1, this._productBuilder.FirstDependency);
                Assert.AreEqual(dependency2, this._productBuilder.SecondDependency);
            }

            [Test]
            public void TwoExternalDependenciesAreUsedByProductWhenPassedInConsecutiveChainedWithCalls()
            {
                // Arrange
                IDependency1 dependency1 = new DummyDependency1();
                IDependency2 dependency2 = new DummyDependency2();

                // Act
                ITestObjBuilder<Product> builder = this._productBuilder.With(FirstDependency => dependency1).
                    With(SecondDependency => dependency2);

                // Assert
                Assert.AreEqual(dependency1, builder.GetType().GetProperty("FirstDependency").GetValue(builder, null));
                Assert.AreEqual(dependency2, builder.GetType().GetProperty("SecondDependency").GetValue(builder, null));
            }
        }
    }
}
