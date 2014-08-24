using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class ProductWithTwoPublicReadWriteProperties
    {
        public ProductWithTwoPublicReadWriteProperties() { }

        public IDependency1 FirstDependency { get; set; }
        public IDependency2 SecondDependency { get; set; }
    }
}
