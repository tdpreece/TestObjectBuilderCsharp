using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class ProductWithPropertyWithPrivateSetter
    {
        public ProductWithPropertyWithPrivateSetter() { }

        public IDependency1 PrivatelySetProperty
        {
            get { return this._privatelySetProperty; }
            private set { this._privatelySetProperty = value; }
        }

        private IDependency1 _privatelySetProperty;
    }
}
