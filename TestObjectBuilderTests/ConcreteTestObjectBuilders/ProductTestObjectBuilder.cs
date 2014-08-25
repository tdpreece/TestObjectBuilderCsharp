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
            this.SecondDependency = new DummyDependency2();
            this.PropertiesUsedByProductConstructor = new List<string>() { "FirstDependency" };
        }

        public IDependency1 FirstDependency { get; set; }
        public IDependency2 SecondDependency { get; set; }
    }
}
