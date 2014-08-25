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

        public static ITestObjBuilder<T> CreateNewObject()
        {
            return CreateNewObject(null);
        }

        /**
         * TODO: 
         * - Create additional properties on the builder for ctorArgs.
         * - Have build method use these args to build the product.
         */ 
        public static ITestObjBuilder<T> CreateNewObject(TestObjectConstructorArgumentList ctorArgs)
        {
            Type finalProductType = typeof(T);
            var myType = CompileResultType(ctorArgs);
            var myObject = Activator.CreateInstance(myType);
            return (ITestObjBuilder<T>)myObject;
        }

        public static Type CompileResultType(TestObjectConstructorArgumentList ctorArgs)
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
                                , typeof(TestObjBuilder<T>));
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

            if (constructorArguments != null)
            {
                foreach (TestObjectConstructorArgument ctorArg in constructorArguments)
                {
                    if ( propertiesToAdd.ContainsKey(ctorArg.ArgumentName) )
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
            }

            return propertiesToAdd;

        }
    }
}
