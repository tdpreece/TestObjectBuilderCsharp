using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class Product
    {
        public Product()
        {

        }

        public Product(IDependency1 firstDependency) 
        { 
            this._firstDependency = firstDependency;
        }

        public IDependency1 FirstDependency
        {
            get { return this._firstDependency; }
            private set { this._firstDependency = value; }
        }

        public IDependency2 SecondDependency { get; set; }

        private IDependency1 _firstDependency;

    }
}
