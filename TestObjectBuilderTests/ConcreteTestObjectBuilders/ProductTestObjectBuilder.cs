using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestObjectBuilder;

namespace TestObjectBuilderTests
{
    public class ProductTestObjectBuilder : TestObjBuilder<Product>
    {
        public ProductTestObjectBuilder()
        {
            this.FirstDependency = new DummyDependency1();
        }

        override public Product Build()
        {
            return new Product(this.FirstDependency);
        }

        public IDependency1 FirstDependency { get; set; }
    }
}
