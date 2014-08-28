using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using TestObjectBuilder;

namespace Examples
{
    [TestFixture]
    class ExampleTests
    {
        [Test, Description("Build a Product using a Test Object Builder that was coded by hand.")]
        public void Example1()
        {
            var builder = new ProductBuilderUsingBaseClass();
            var aList = new List<int>() { 2, 3 };
            builder.With(X => 1, Y => aList);

            var aClassInstance = builder.Build();

            Assert.AreEqual(aList, aClassInstance.Y);
            Assert.AreEqual(1, aClassInstance.X);
        }

        [Test, Description("Build a Test Object Builder at run time.")]
        public void Example2()
        {
            // The following code creates a builder class at run time that is the 
            // same as ProductBuilder.
            var constructorArguments = new TestObjectConstructorArgumentList() {
                new TestObjectConstructorArgument("X", typeof(int))
            };
            var builder = TestObjectBuilderBuilder<Product>.Build(
                constructorArguments);

            var aList = new List<int>() { 2, 3 };
            builder.With(X => 1, Y => aList);
            
            var aClassInstance = builder.Build();
            
            Assert.AreEqual(aList, aClassInstance.Y);
            Assert.AreEqual(1, aClassInstance.X);
        }
    }
}
