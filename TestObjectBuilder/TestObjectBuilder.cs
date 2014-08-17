using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilder
{
    public abstract class TestObjBuilder<T> : ITestObjBuilder<T>
    {
        public abstract T Build();

        // <summary>
        // Returns a memberwise clone of the current builder.
        // <remarks>
        // Don't need deep copy as will only be changing what the builder's properties
        // point to not the data they point to.
        public ITestObjBuilder<T> But()
        {
            return (ITestObjBuilder<T>)this.MemberwiseClone();
        }

        // <summary>
        // Sets the property named to the value supplied and returns this so 
        // calls can be chained. 
        // <remarks>
        // Inspired by:
        // http://anaykamat.com/2009/08/09/simple-equivalent-of-with-statement-in-c-sharp/
        public ITestObjBuilder<T> With(params Func<string, object>[] hash)
        {
            foreach (Func<string, object> member in hash)
            {
                var propertyName = member.Method.GetParameters()[0].Name;
                var propertyValue = member(string.Empty);
                this.GetType()
                    .GetProperty(propertyName)
                        .SetValue(this, propertyValue, null);
            };

            return (ITestObjBuilder<T>)this;
        }
    }
}
