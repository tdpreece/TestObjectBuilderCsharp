using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class ProductWithTwoConstructorArgumentsOfDifferentType
    {
        ProductWithTwoConstructorArgumentsOfDifferentType(Dependency1 x, Dependency2 y)
        {
            this._x = x;
            this._y = y;
        }

        private Dependency1 _x; 
        private Dependency2 _y;
    }
}
