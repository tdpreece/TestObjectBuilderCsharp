using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace TestObjectBuilder
{
    public abstract class TestObjectBuilder<T> : ITestObjectBuilder<T>
    {

        #region "Constructors"
        public TestObjectBuilder()
        {
            this.PropertiesUsedByProductConstructor = new List<string>();
            this._propertiesChangedByClient = new HashSet<string>();
        }
        #endregion


        #region "Public Methods"
        public virtual T Build()
        {
            ValidateConstructorArgumentsHaveBeenSet();
            T product;
            try
            {
                if (null == this.PropertiesUsedByProductConstructor ||
                    0 == this.PropertiesUsedByProductConstructor.Count())
                {
                    product = (T)Activator.CreateInstance(typeof(T));
                }
                else
                {
                    product = (T)Activator.CreateInstance(typeof(T), this.GetConstructorArguments());
                }
            }
            catch (MissingMethodException)
            {
                throw new MissingMethodException(this.GetConstructorErrorString());
            }


            InitialiseProductProperties(product);
            return product;
        }

        // <summary>
        // Returns a memberwise clone of the current builder.
        // <remarks>
        // Don't need deep copy as will only be changing what the builder's properties
        // point to not the data they point to.
        public ITestObjectBuilder<T> But()
        {
            return (ITestObjectBuilder<T>)this.MemberwiseClone();
        }

        // <summary>
        // Sets the property named to the value supplied and returns this so 
        // calls can be chained. 
        // <remarks>
        // Inspired by:
        // http://anaykamat.com/2009/08/09/simple-equivalent-of-with-statement-in-c-sharp/
        public ITestObjectBuilder<T> With(params Func<string, object>[] hash)
        {
            foreach (Func<string, object> member in hash)
            {
                var propertyName = member.Method.GetParameters()[0].Name;
                var propertyValue = member(string.Empty);
                GetPropertyInfoForProperty(propertyName).SetValue(this, propertyValue, null);
                _propertiesChangedByClient.Add(propertyName);
            };

            return (ITestObjectBuilder<T>)this;
        }

        /// <summary>
        /// Gets value of property requested.
        /// </summary>
        /// <param name="propertyName">property name as string</param>
        /// <returns></returns>
        public object GetPropertyValue(string propertyName)
        {
            return this.GetPropertyInfoForProperty(propertyName).GetValue(this, null);
        }
        #endregion

        #region "Public Properties"
        public List<string> PropertiesUsedByProductConstructor { get; set; }
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

        /// <summary>
        /// Set properties on product to value of property with matching name on the 
        /// TestObjectBuilder for properties that i) are not arguments to the products
        /// constructor and ii) whose values have been set by the client.
        /// </summary>
        /// <param name="product">product in which to set properties.</param>
        protected void InitialiseProductProperties(T product)
        {
            HashSet<string> properteiesToSetOnProduct = new HashSet<string>();
            properteiesToSetOnProduct.UnionWith(this._propertiesChangedByClient);
            properteiesToSetOnProduct.ExceptWith(this.PropertiesUsedByProductConstructor);

            foreach (string propertyName in properteiesToSetOnProduct)
            {
                object builderPropertyValue = GetPropertyInfoForProperty(propertyName).GetValue(this, null);
                PropertyInfo propertyInfo = product.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(product, builderPropertyValue, null);
            }
        }

        /// <summary>
        /// Validate that user has set property values for all properties that
        /// are used to construct the test object.
        /// </summary>
        protected void ValidateConstructorArgumentsHaveBeenSet()
        {
            foreach (string propertyName in this.PropertiesUsedByProductConstructor)
            {
                if (false == _propertiesChangedByClient.Contains(propertyName))
                {
                    throw new ArgumentException(string.Format("Constructor argument {0} " +
                        "has not been set and is required to construct the test object, {1}",
                        propertyName, typeof(T).ToString()));
                }
            }
        }

        protected object[] GetConstructorArguments()
        {
            List<object> ctorValues = new List<object>();
            foreach (string argName in this.PropertiesUsedByProductConstructor)
                ctorValues.Add(this.GetPropertyValue(argName));
            object[] ctorArgs = ctorValues.ToArray();
            return ctorArgs;
        }

        private string GetConstructorErrorString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("No matching constructor found for, ");
            sb.Append(typeof(T).ToString());
            sb.Append("(");
            foreach (object arg in this.GetConstructorArguments())
            {
                sb.Append(arg.GetType().ToString());
            }
            sb.Append(").");
            
            return sb.ToString();
        }
        #endregion

        #region "private members"
        HashSet<string> _propertiesChangedByClient;
        #endregion
    }
}
