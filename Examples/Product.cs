using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    public class Product
    {
        public Product(int x)
        {
            this._x = x;
        }

        public int X { get { return this._x; } }

        public List<int> Y { get; set; }

        private int _x;
    }
}
