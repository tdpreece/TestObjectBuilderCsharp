using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestObjectBuilder;

namespace TestObjectBuilderTests
{
    public class ProductTestObjectBuilder : TestObjBuilder<Product>
    {
        public ProductTestObjectBuilder()
        {
            this.FirstDependency = new DummyDependency1();
            this.SecondDependency = new DummyDependency2();
            this.ConstructorArgumentPropertyNames = new List<string>() { "FirstDependency" };
        }

        override public Product Build()
        {
            if (this.ConstructorArgumentPropertyNames.Count() == 0)
            {
                return new Product(this.FirstDependency);
            }
            else
            {
                List<object> ctorValues = new List<object>();
                foreach (string argName in this.ConstructorArgumentPropertyNames)
                    ctorValues.Add(this.GetProperty(argName));
                object[] ctorArgs = ctorValues.ToArray();
                return (Product)Activator.CreateInstance(typeof(Product), ctorArgs);
            }
        }

        public List<string> ConstructorArgumentPropertyNames { get; set; }

        public IDependency1 FirstDependency { get; set; }
        public IDependency2 SecondDependency { get; set; }
    }
}
