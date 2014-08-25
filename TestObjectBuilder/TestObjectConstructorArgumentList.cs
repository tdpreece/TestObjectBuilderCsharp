using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilder
{
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

        /// <summary>
        /// Returns true if a TestObjectConstructorArgument with matching name
        /// exists in this list otherwise returns false.
        /// </summary>
        /// <param name="name">Name of constructor argument.</param>
        /// <returns>
        /// true if a TestObjectConstructorArgument with matching name exists in this list otherwise returns false.
        /// </returns>
        public bool ContainsArgumentWithName(string name)
        {
            foreach (TestObjectConstructorArgument arg in List)
            {
                if (name == arg.ArgumentName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a TestObjectConstructorArgument with a matching name or
        /// null if no match is found.
        /// </summary>
        /// <param name="name">Name of constructor argument to search for.</param>
        /// <returns>TestObjectConstructorArgument or null if no match is found.</returns>
        public TestObjectConstructorArgument GetArgumentByName(string name)
        {
            foreach (TestObjectConstructorArgument arg in List)
            {
                if (name == arg.ArgumentName)
                {
                    return arg;
                }
            }
            return null;
        }
    }
}
