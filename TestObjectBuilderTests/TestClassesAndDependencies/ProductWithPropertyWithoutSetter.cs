using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class ProductWithPropertyWithoutSetter
    {
        public ProductWithPropertyWithoutSetter() 
        {
            this._firstDependency = new Dependency1();
        }

        public IDependency1 PropertyWithoutSetter
        {
            get { return this._firstDependency; }
        }

        private IDependency1 _firstDependency;
    }
}
