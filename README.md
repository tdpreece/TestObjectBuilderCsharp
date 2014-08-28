TestObjectBuildersCsharp
========================
TestObjectBuildersCsharp allows you to quickly create builder 
classes that put together data for classes under test and/or their 
collaborators.

# Installation
* Download the TestObjectBuilder project and add to your solution.
* Reference TestObjectBuilder project from the project that contains your unit tests.

# Background
I liked Nat Pryce's Test Data Builders (http://www.natpryce.com/articles/000714.html) 
for instantiating classes in unit tests, however, I didn't like writing 
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
property values that should be passed to the constructor when a Product is
instantiated.  This can be used like,
```
            var builder = new ProductBuilderUsingBaseClass();
            var aList = new List<int>() { 2, 3 };
            builder.With(X => 1, Y => aList);

            var aClassInstance = builder.Build();

            Assert.AreEqual(aList, aClassInstance.Y);
            Assert.AreEqual(1, aClassInstance.X);
```

If this is still too much work this Project you can make use of the 
TestObjectBuilderBuilder, which creates TestObjectBuilders for you
at run time.  The following code creates a TestObjectBuilder that 
looks the same as ProductBuilderUsingBaseClass.
```
            var constructorArguments = new TestObjectConstructorArgumentList() {
                new TestObjectConstructorArgument("X", typeof(int))
            };
            var builder = TestObjectBuilderBuilder<Product>.Build(
                constructorArguments);

            var aList = new List<int>() { 2, 3 };
            builder.With(X => 1, Y => aList);
            
            var aClassInstance = builder.Build();
            
            Assert.AreEqual(aList, aClassInstance.Y);
            Assert.AreEqual(1, aClassInstance.X);
```
1 The TestObjectBuilderBuilder first adds properties to the builder for every 
property on Product that has a setter defined.
2 It then adds additional properties for any constructor arguments specified
that don't share a name with any of Properties added in the previous step.
3 It then sets PropertiesUsedByProductConstructor property using the list of
constructor arguments specified.





