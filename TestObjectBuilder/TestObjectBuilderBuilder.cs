using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Reflection.Emit;

namespace TestObjectBuilder
{
    /**
     * 
     * Taken from: 
     * http://stackoverflow.com/questions/3862226/dynamically-create-a-class-in-c-sharp
     * http://stackoverflow.com/questions/17519078/initializing-a-generic-variable-from-a-c-sharp-type-variable
     */
    public class TestObjectBuilderBuilder<T>
    {
        #region "Public Functions"
        /// <summary>
        /// Builds a TestObjectBuilder for type T.
        /// </summary>
        /// <returns>instance of TestObjectBuilder for type T</returns>
        public static ITestObjectBuilder<T> Build()
        {
            return Build(null);
        }

        /// <summary>
        /// Builds a TestObjectBuilder for type T.
        /// </summary>
        /// <param name="ctorArgs">
        /// List of arguments used to construct object of type T.
        /// </param>
        /// <returns>instance of TestObjectBuilder for type T</returns>
        public static ITestObjectBuilder<T> Build(TestObjectConstructorArgumentList ctorArgs)
        {
            if (null == ctorArgs)
            {
                ctorArgs = new TestObjectConstructorArgumentList();
            }
            ValidateConstructorArguments(ctorArgs);
            var myType = CompileResultType(ctorArgs);
            ITestObjectBuilder<T> testObjectBuilder = (ITestObjectBuilder<T>)Activator.CreateInstance(myType);
            testObjectBuilder.PropertiesUsedByProductConstructor = GetNamesProductConstructorArguements(ctorArgs);
            return testObjectBuilder;
        }

        #endregion

        #region "Private Functions"
        private static Type CompileResultType(TestObjectConstructorArgumentList ctorArgs)
        {
            TypeBuilder tb = GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            Dictionary<string, Type> propertiesToAdd = GetPropertiesToAddToBuilder(ctorArgs);
            foreach (KeyValuePair<string, Type> property in propertiesToAdd)
            {
                CreateProperty(tb, property.Key, property.Value);
            }

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder()
        {
            var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , typeof(TestObjectBuilder<T>));
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | 
                MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }

        /// <summary>
        /// Inspects the product type and client supplied constructor arguments and 
        /// retruns a list of properties that should be added to a TestObjectBuilder 
        /// for the product. 
        /// </summary>
        /// <param name="constructorArguments">TestObjectConstructorArgumentList</param>
        /// <returns>Dictionary of {property name, property type}</returns>
        private static Dictionary<string, Type> GetPropertiesToAddToBuilder(TestObjectConstructorArgumentList constructorArguments)
        {
            Dictionary<string, Type> propertiesToAdd = new Dictionary<string, Type>();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                /** 
                 * Need setter to be definied on a product in order to set 
                 * the value after it has been constructed.
                 */
                if (propertyInfo.CanWrite)
                {
                    propertiesToAdd.Add(propertyInfo.Name, propertyInfo.PropertyType);
                }
            }


            foreach (TestObjectConstructorArgument ctorArg in constructorArguments)
            {
                if (propertiesToAdd.ContainsKey(ctorArg.ArgumentName))
                {
                    if (propertiesToAdd[ctorArg.ArgumentName] == ctorArg.ArgumentType)
                    {
                        continue;
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("Constructor argument '{0}' of type {1} " +
                            "has same name but different type to a property of {2}.  The name of the " +
                            "constructor argument should be changed.", ctorArg.ArgumentName,
                            ctorArg.ArgumentType, typeof(T)));
                    }
                }
                else
                {
                    propertiesToAdd.Add(ctorArg.ArgumentName, ctorArg.ArgumentType);
                }
            }


            return propertiesToAdd;

        }

        /// <summary>
        /// Returns a list of Names for the TestObjectConstructorArgumentList supplied.
        /// </summary>
        /// <param name="ctorArgs">TestObjectConstructorArgumentList</param>
        /// <returns>List of property name strings.</returns>
        private static List<string> GetNamesProductConstructorArguements(TestObjectConstructorArgumentList ctorArgs)
        {
            List<string> propertyNames = new List<string>();

            foreach (TestObjectConstructorArgument arg in ctorArgs)
            {
                propertyNames.Add(arg.ArgumentName);
            }


            return propertyNames;
        }

             
        /// <summary>
        /// Validates that we don't have multiple constructor arguments with the
        /// same name but different type.
        /// </summary>
        private static void ValidateConstructorArguments(TestObjectConstructorArgumentList ctorArgs)
        {
            foreach (TestObjectConstructorArgument arg1 in ctorArgs)
            {
                foreach (TestObjectConstructorArgument arg2 in ctorArgs)
                {
                    if (arg1.ArgumentName == arg2.ArgumentName
                        && arg1.ArgumentType != arg2.ArgumentType)
                    {
                        throw new ArgumentException(String.Format(
                            "Two constructor argument have same name, '{0}', but " +
                            "different types, {1} and {2}.  Change the name of one.",
                             arg1.ArgumentName, arg1.ArgumentType, arg2.ArgumentType));
                    }
                }
            }
        }
        #endregion
    }
}
