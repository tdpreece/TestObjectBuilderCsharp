using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace TestObjectBuilder
{
    public abstract class TestObjBuilder<T> : ITestObjBuilder<T>
    {

        #region "Constructors"
        public TestObjBuilder()
        {
            this.InitialiseBuilderProperties();
        }
        #endregion


        #region "Public Methods"
        public virtual T Build()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

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
                GetPropertyInfoForProperty(propertyName).SetValue(this, propertyValue, null);
            };

            return (ITestObjBuilder<T>)this;
        }

        // <summary>
        // Gets value of property requested.
        // <remards>
        // Added so that a user can easity get property values of property through ITestObjectBuilder.
        // Thus, user doesn't have to cast to a concrete class of the TestObjectBuilder.
        public object GetProperty(string propertyName)
        {
            return this.GetPropertyInfoForProperty(propertyName).GetValue(this, null);
        }
        #endregion

        #region "protected methods"
        protected PropertyInfo GetPropertyInfoForProperty(string propertyName)
        {
            Type builderType = this.GetType();
            PropertyInfo propertyInfo = builderType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                string propertyNotImplementedErrorMsg =
                    String.Format("Object '{0}' does not implement a property called '{1}'",
                    builderType.Name, propertyName);
                throw new ArgumentException(propertyNotImplementedErrorMsg);
            }
            return propertyInfo;
        }

        protected void InitialiseBuilderProperties()
        {
            PropertyInfo[] propertyInfos;
            propertyInfos = this.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyInfo.SetValue(this, null, null);
            }
        }

        #endregion
    }
}
