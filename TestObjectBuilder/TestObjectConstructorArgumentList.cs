using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilder
{
    /// <summary>
    /// An ordered list of TestObjectConstructorArguments.
    /// </summary>
    public class TestObjectConstructorArgumentList : System.Collections.CollectionBase
    {
        /// <summary>
        /// Add TestObjectConstructorArgument to this.
        /// </summary>
        /// <param name="argument">TestObjectConstructorArgument</param>
        public void Add(TestObjectConstructorArgument argument)
        {
            List.Add(argument);
        }

        /// <summary>
        /// Returns TestObjectConstructorArgument at given index in list.
        /// </summary>
        /// <param name="index">index of item in list.</param>
        /// <returns>TestObjectConstructorArgument</returns>
        public TestObjectConstructorArgument Item(int index)
        {
            return (TestObjectConstructorArgument)List[index];
        }
    }
}
