using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using TestObjectBuilder;
using TestObjectBuilderTests;

namespace Examples
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ITestObjBuilder<Product> myBuilder = TestObjectBuilderBuilder<Product>.CreateNewObject();
            Console.WriteLine(myBuilder.GetType());
            PropertyInfo[] propertyInfos;
            propertyInfos = myBuilder.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Console.WriteLine(propertyInfo.Name);
                Console.WriteLine(propertyInfo.PropertyType);
                Console.WriteLine(propertyInfo.GetValue(myBuilder, null));
            }

            Product p = myBuilder.Build();
            Console.WriteLine((p.FirstDependency == null));
            Console.WriteLine((p.SecondDependency == null));

            DummyDependency1 externallySuppliedDependency1 = new DummyDependency1();
            Product p2 = myBuilder.With(FirstDependency => externallySuppliedDependency1).Build();
            Console.WriteLine((p2.FirstDependency == null));
            Console.WriteLine((p2.SecondDependency == null));

            Console.ReadKey();
        }
    }
}
