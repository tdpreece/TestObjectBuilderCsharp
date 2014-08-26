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
            this.PropertiesUsedByProductConstructor = new List<string>() { "FirstDependency" };
        }

        public IDependency1 FirstDependency { get; set; }
        public IDependency2 SecondDependency { get; set; }
        public IDependency1 ThirdDependency { get; set; }
    }
}
