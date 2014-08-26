using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilder
{
    public interface ITestObjBuilder<T>
    {
        T Build();
        ITestObjBuilder<T> But();
        ITestObjBuilder<T> With(params Func<string, object>[] hash);
        object GetProperty(string propertyName);

        List<string> PropertiesUsedByProductConstructor { get; set; }
    }
}
