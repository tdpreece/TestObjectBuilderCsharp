using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilder
{
    /// <summary>
    /// Defines the name and type of a constructor argument.
    /// </summary>
    public class TestObjectConstructorArgument
    {
        public TestObjectConstructorArgument(string argName, Type argType)
        {
            this.ArgumentName = argName;
            this.ArgumentType = argType;
        }

        public string ArgumentName { get; set; }
        public Type ArgumentType { get; set; }
    }
}
