using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TestObjectBuilder;

namespace Examples
{
    public class ProductBuilder : TestObjectBuilder<Product>
    {
        public ProductBuilder()
        {
            this.PropertiesUsedByProductConstructor = new List<string>() { "X" };
        }

        public int X { get; set; }
        public List<int> Y { get; set; }
    }
}
