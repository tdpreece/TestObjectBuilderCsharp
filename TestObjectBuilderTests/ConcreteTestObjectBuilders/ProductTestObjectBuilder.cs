using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestObjectBuilder;

namespace TestObjectBuilderTests
{
    class ProductTestObjectBuilder : TestObjBuilder<Product>
    {
        public ProductTestObjectBuilder()
        {
            this.FirstDependency = new DummyDependency1();
        }

        override public Product Build()
        {
            return new Product(this.FirstDependency);
        }

        private IDependency1 FirstDependency { get; set; }
    }
}
