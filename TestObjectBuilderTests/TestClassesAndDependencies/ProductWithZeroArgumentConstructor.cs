using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class ProductWithZeroArgumentConstructor
    {
        public ProductWithZeroArgumentConstructor() { }

        public IDependency1 FirstDependency { get; set; }
    }
}
