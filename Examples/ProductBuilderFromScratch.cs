using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    class ProductBuilderFromScratch
    {
        Product Build()
        {
            return new Product(this.X);
        }

        ProductBuilderFromScratch With(int x)
        {
            this.X = x;
            return this;
        }

        ProductBuilderFromScratch With(List<int> y)
        {
            this.Y = y;
            return this;
        }

        ProductBuilderFromScratch But()
        {
            return (ProductBuilderFromScratch)this.MemberwiseClone();
        }

        public int X { get; set; }
        public List<int> Y { get; set; }
    }
}
