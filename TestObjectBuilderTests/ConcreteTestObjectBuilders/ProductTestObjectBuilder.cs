using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestObjectBuilder;

namespace TestObjectBuilderTests
{
    public class ProductTestObjectBuilder : TestObjectBuilder<Product>
    {
        public ProductTestObjectBuilder()
        {
        }

        public IDependency1 FirstDependency { get; set; }
        public IDependency2 SecondDependency { get; set; }
        public IDependency1 ThirdDependency { get; set; }
    }
}
