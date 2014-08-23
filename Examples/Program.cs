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
            object myBuilder = TestObjectBuilderBuilder.CreateNewObject(typeof(Product));
            Console.WriteLine(myBuilder.GetType());
            PropertyInfo[] propertyInfos;
            propertyInfos = myBuilder.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Console.WriteLine(propertyInfo.Name);
                Console.WriteLine(propertyInfo.PropertyType);
            }

            Console.ReadKey();
        }
    }
}
