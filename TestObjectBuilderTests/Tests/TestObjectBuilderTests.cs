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
     * - Add a TestBuilderFactory class
     * - with()
     * - but()
     * - build()
     * - product is simple class
     * - product uses inheritance
     */ 
    public class TestObjectBuilderTests
    {
        [TestFixture]
        public class With
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
    }
}
