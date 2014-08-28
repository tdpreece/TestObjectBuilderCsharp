using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilder
{
    public interface ITestObjectBuilder<T>
    {
        T Build();
        ITestObjectBuilder<T> But();
        ITestObjectBuilder<T> With(params Func<string, object>[] hash);
        object GetProperty(string propertyName);

        List<string> PropertiesUsedByProductConstructor { get; set; }
    }
}
