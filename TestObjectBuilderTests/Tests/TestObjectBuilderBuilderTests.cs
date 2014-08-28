using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using NUnit.Framework;

using TestObjectBuilder;

namespace TestObjectBuilderTests.Tests
{
    public class TestObjectBuilderBuilderTests
    {
        [TestFixture]
        public class CreateNewObject
        {
            [Test]
            public void ProductBuilderCreateBuildsObjectsOfTypeProduct()
            {
                // Arrange
                // Act
                ITestObjectBuilder<ProductWithoutProperties> builder =
    TestObjectBuilderBuilder<ProductWithoutProperties>.Build();
                // Assert
                Assert.AreSame(typeof(ProductWithoutProperties), builder.GetType().GetMethod("Build").ReturnType);
            }

            [Test]
            public void ProductBuilderHasSamePublicReadWriteProperitesAsProduct()
            {
                // Arrange
                // Act
                ITestObjectBuilder<ProductWithTwoPublicReadWriteProperties> builder =
    TestObjectBuilderBuilder<ProductWithTwoPublicReadWriteProperties>.Build();

                // Assert
                List<Tuple<string, Type, bool>> builderProperties = GetListOfPropertyNameTypeAccessibility(builder.GetType());

                Assert.Contains(Tuple.Create<string, Type, bool>("FirstDependency", typeof(IDependency1), true), 
                    builderProperties);
                Assert.Contains(Tuple.Create<string, Type, bool>("SecondDependency", typeof(IDependency2), true), 
                    builderProperties);
            }

            [Test]
            public void ProductBuilderHasPublicReadWritePropertyWhenProductHasPrivatelySetProperty()
            {
                // Arrange
                String privatelySetPropertyName = "PrivatelySetProperty";

                // Preconditions
                PropertyInfo productPrivateProperty = typeof(ProductWithPropertyWithPrivateSetter).
                    GetProperty(privatelySetPropertyName);
                Assert.IsTrue(productPrivateProperty.CanWrite); // A setter exists.
                Assert.IsNull(productPrivateProperty.GetSetMethod()); // Public setter doesn't exist.

                // Act
                ITestObjectBuilder<ProductWithPropertyWithPrivateSetter> builder =
                    TestObjectBuilderBuilder<ProductWithPropertyWithPrivateSetter>.Build();

                // Assert
                // Tuple<property name, type, has public setter>
                List<Tuple<string, Type, bool>> builderProperties = GetListOfPropertyNameTypeAccessibility(builder.GetType());
                Tuple<string, Type, bool> propertyPrivateInProductPublicInBuilder = 
                    Tuple.Create<string, Type, bool>("PrivatelySetProperty", typeof(IDependency1), true);
                Assert.Contains(propertyPrivateInProductPublicInBuilder, builderProperties);
            }

            [Test]
            public void BuilderDoesNotHavePropertyWhenProductPropertyDoesNotHaveSetter()
            {
                // Arrange
                String propertyWithoutSetterName = "PropertyWithoutSetter";

                // Preconditions
                PropertyInfo productPrivateProperty = typeof(ProductWithPropertyWithoutSetter).
                    GetProperty(propertyWithoutSetterName);
                Assert.IsFalse(productPrivateProperty.CanWrite); // A setter exists.

                // Act
                ITestObjectBuilder<ProductWithPropertyWithoutSetter> builder =
    TestObjectBuilderBuilder<ProductWithPropertyWithoutSetter>.Build();

                // Assert
                List<Tuple<string, Type, bool>> builderProperties = GetListOfPropertyNameTypeAccessibility(builder.GetType());
                    Assert.IsNull(builder.GetType().GetProperty(propertyWithoutSetterName));
            }

            [Test]
            public void BuilderHasPropertiesForConstructorArgumentsSpecified()
            {
                // Arrange
                TestObjectConstructorArgument arg1 = 
                    new TestObjectConstructorArgument("Arg1", typeof(IDependency1));
                TestObjectConstructorArgument arg2 =
                    new TestObjectConstructorArgument("Arg2", typeof(IDependency1));
                TestObjectConstructorArgumentList constructorArguments =
                    new TestObjectConstructorArgumentList() { arg1, arg2 };

                // Act
                ITestObjectBuilder<ProductWithoutProperties> builder =
    TestObjectBuilderBuilder<ProductWithoutProperties>.Build(constructorArguments);
                
                // Assert
                List<Tuple<string, Type, bool>> builderProperties = GetListOfPropertyNameTypeAccessibility(builder.GetType());
                Assert.Contains(Tuple.Create(arg1.ArgumentName, arg1.ArgumentType, true), builderProperties);
                Assert.Contains(Tuple.Create(arg2.ArgumentName, arg2.ArgumentType, true), builderProperties);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void BuilderBuilderThrowsExceptionWhenAConstructorArgSharesANameButNotTypeWithAProductProperty()
            {
                // Arrange
                TestObjectConstructorArgument arg1 = 
                    new TestObjectConstructorArgument("FirstDependency", typeof(IDependency2));
                TestObjectConstructorArgumentList constructorArguments =
                    new TestObjectConstructorArgumentList() { arg1 };

                // Act
                ITestObjectBuilder<ProductWithTwoPublicReadWriteProperties> builder =
                    TestObjectBuilderBuilder<ProductWithTwoPublicReadWriteProperties>.Build(constructorArguments);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void BuilderBuilderThrowsExceptionWhenTwoConstructorArgsSharesANameButNotType()
            {
                // Arrange
                TestObjectConstructorArgument arg1 =
                    new TestObjectConstructorArgument("Arg1", typeof(Dependency1));
                TestObjectConstructorArgument arg2 =
                    new TestObjectConstructorArgument("Arg1", typeof(Dependency2));
                TestObjectConstructorArgumentList constructorArguments =
                    new TestObjectConstructorArgumentList() { arg1, arg2 };

                // Act
                ITestObjectBuilder<ProductWithTwoConstructorArgumentsOfDifferentType> builder =
                    TestObjectBuilderBuilder<ProductWithTwoConstructorArgumentsOfDifferentType>.
                    Build(constructorArguments);
            }

            [Test]
            public void PropertiesUsedByProductConstructorArePopulatedWhenConstructorArgsAreSuppliedByClient()
            {
                
                // Arrange
                TestObjectConstructorArgument arg1 = 
                    new TestObjectConstructorArgument("Arg1", typeof(IDependency1));
                TestObjectConstructorArgument arg2 =
                    new TestObjectConstructorArgument("Arg2", typeof(IDependency1));
                TestObjectConstructorArgumentList constructorArguments =
                    new TestObjectConstructorArgumentList() { arg1, arg2 };

                // Act
                ITestObjectBuilder<ProductWithoutProperties> builder =
    TestObjectBuilderBuilder<ProductWithoutProperties>.Build(constructorArguments);

                // Assert
                Assert.IsNotNull(builder.PropertiesUsedByProductConstructor);
                Assert.AreEqual(2, builder.PropertiesUsedByProductConstructor.Count());
                Assert.Contains(arg1.ArgumentName, builder.PropertiesUsedByProductConstructor);
                Assert.Contains(arg2.ArgumentName, builder.PropertiesUsedByProductConstructor);
            }

            [Test]
            public void OnlyOnePropertyAddedToBuilderForTwoConstructorArgsWithSameName()
            {
                // This isn't something I'd expect someone to want to do but I don't want this to 
                // break if they do.
                
                // Arrange
                TestObjectConstructorArgument arg1 = new TestObjectConstructorArgument("Arg1", typeof(Dependency1));
                TestObjectConstructorArgument arg2 = new TestObjectConstructorArgument("Arg1", typeof(Dependency1));
                TestObjectConstructorArgumentList constructorArguments = new TestObjectConstructorArgumentList() {
                    arg1, arg2 };

                // Act
                ITestObjectBuilder<ProductWithTwoIdenticalConstructorArguments> builder =
                    TestObjectBuilderBuilder<ProductWithTwoIdenticalConstructorArguments>.Build(
                    constructorArguments);

                // Assert
                Assert.IsNotNull(builder.GetType().GetProperty("Arg1"));
                // Can't have two properties with the same name so would get an error if tries to add two.
            }

            #region "private helper methods"
            /**
             * <summary>
             * Returns a list list of tuples which describe the name, type
             * and setter accessibility for each property of the type provided.
             * </summary>
             * <returns>
             * List of Tuples of property name, type, property has public setter?
             * </returns>
             */
            private List<Tuple<string, Type, bool>> GetListOfPropertyNameTypeAccessibility(Type inputType)
            {
                PropertyInfo[] propertyInfos;
                propertyInfos = inputType.GetProperties();
                List<Tuple<string, Type, bool>> propertiesList = new List<Tuple<string, Type, bool>>();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    propertiesList.Add(Tuple.Create<string, Type, bool>(
                        propertyInfo.Name,
                        propertyInfo.PropertyType,
                        propertyInfo.GetSetMethod() != null));
                }

                return propertiesList;
            }
            #endregion
        }
    }
}
