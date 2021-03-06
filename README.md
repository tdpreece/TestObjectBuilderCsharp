TestObjectBuildersCsharp
========================
The aim of this project is to allow you to quickly create builder 
classes that put together test data for classes under test and/or their 
collaborators.

I started work on this project before I was aware of https://github.com/AutoFixture/AutoFixture,
though I still think that this project is useful for situations where
you require greater control of which constructor is used in the class being built
( see limitations to Autofixture in this respect: http://blog.ploeh.dk/2009/03/24/HowAutoFixtureCreatesObjects/). 

# Installation
This is still work in progress so the installation is a little manual.
* Download the TestObjectBuilder project and add to your solution.
* Reference TestObjectBuilder project from the project that contains your unit tests.

# Background
I like Nat Pryce's Test Data Builders (http://www.natpryce.com/articles/000714.html) 
for instantiating classes in unit tests, however, I don't like writing 
a builder for each class.

Consider the following class,
```
    public class Product
    {
        public Product(int x)
        {
            this._x = x;
        }

        public int X { get { return this._x; } }

        public List<int> Y { get; set; }

        private int _x;
    }
```
A builder to create test data for this class would probably look something like,
```
    class ProductBuilderFromScratch
    {
        Product Build()
        {
            return new Product(this.X);
        }

        ProductBuilderFromScratch With(int x)
        {
            this.X = x;
            return this;
        }

        ProductBuilderFromScratch With(List<int> y)
        {
            this.Y = y;
            return this;
        }

        ProductBuilderFromScratch But()
        {
            return (ProductBuilderFromScratch)this.MemberwiseClone();
        }

        public int X { get; set; }
        public List<int> Y { get; set; }
    }
```
This project contains a TestObjectBuilder class which you can subclass to 
reduce the amount of code you have to manually enter for each builder.
For example,
```
    public class ProductBuilderUsingBaseClass : TestObjectBuilder<Product>
    {
        public ProductBuilderUsingBaseClass()
        {
            this.PropertiesUsedByProductConstructor = new List<string>() { "X" };
        }

        public int X { get; set; }
        public List<int> Y { get; set; }
    }
```
The PropertiesUsedByProductConstructor property is an ordered list of the 
property values that will be passed to the constructor when a Product is
instantiated.  ProductBuilderUsingBaseClass can be used like,
```
            var builder = new ProductBuilderUsingBaseClass();
            var aList = new List<int>() { 2, 3 };
            builder.With(X => 1, Y => aList);

            var aClassInstance = builder.Build();

            Assert.AreEqual(aList, aClassInstance.Y);
            Assert.AreEqual(1, aClassInstance.X);
```
1. When builder.Build() is called a Product is instantiated with the value of 
property X passed to the constructor.
2. Property Y on the instance is then set to the value of Y on the builder.  NOTE
that this only happened because Y had been set on the builder using the With method.
If this had not been done, Y would not have been set on the instance.  The builder
will only set values on the product that have been set on the builder using the With
method.

This project also contains a TestObjectBuilderBuilder class that
can create TestObjectBuilders at run time.  This is work in progress.
Currently a new Dynamic Assembly is created for each dynamically created builder.
An alternative would be to create a dynamic assembly once and use it again and again.  
This also allows you to cache dynamic types that you've built once already.  See also,
TestObjectBuilderCsharp/FutureEnhancements/FutureEnhancements.txt.

 
The following code creates a TestObjectBuilder that 
has the same structure as ProductBuilderUsingBaseClass described above.
```
            var constructorArguments = new TestObjectConstructorArgumentList() {
                new TestObjectConstructorArgument("X", typeof(int))
            };
            var builder = TestObjectBuilderBuilder<Product>.Build(
                constructorArguments);
```
1. The TestObjectBuilderBuilder first adds properties to the builder for every 
property on Product that has a setter defined.
2. It then adds additional properties for any constructor arguments specified
that don't share a name with any of Properties added in the previous step.
3. It then sets PropertiesUsedByProductConstructor property using the list of
constructor arguments specified.




