using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilder
{
    public interface ITestObjectBuilder<T>
    {
        /// <summary>
        /// Instantiates object of type T using the constructor which matches
        /// this.PropertiesUsedByProductConstructor.
        /// After instantiating the object, any properties that have been set
        /// on the builder will be set to the same values on the object instance
        /// (this excludes properties listed in 
        /// this.PropertiesUsedByProductConstructor).
        /// </summary>
        /// <returns>An instance of type T</returns>
        T Build();

        /// <summary>
        /// Returns a memberwise clone of the current builder.
        /// </summary>
        /// <returns>MemberwiseClone of this</returns>
        ITestObjectBuilder<T> But();

        /// <summary>
        /// Set property values on this builder.
        /// </summary>
        /// <param name="hash">E.G. Id => 4, Name => "Tim"</param>
        /// <returns>this builder</returns>
        ITestObjectBuilder<T> With(params Func<string, object>[] hash);

        /// <summary>
        /// Gets value of property requested.
        /// </summary>
        /// <param name="propertyName">property name as string</param>
        /// <returns>property value</returns>
        object GetPropertyValue(string propertyName);

        /// <summary>
        /// An ordered list of the names of properties on the builder 
        /// that should be passed to the products constructor in the Build() 
        /// method.
        /// </summary>
        List<string> PropertiesUsedByProductConstructor { get; set; }
    }
}
