using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class ProductWithTwoIdenticalConstructorArguments
    {
        ProductWithTwoIdenticalConstructorArguments(Dependency1 x, Dependency1 y)
        {
            this._x = x;
            this._y = y;
        }

        private Dependency1 _x;
        private Dependency1 _y;
    }
}
