using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestObjectBuilderTests
{
    public class Product
    {
        public Product()
        {
        }

        public Product(IDependency1 firstDependency) 
        {
            this.constructorArgsUsed.Add(firstDependency);
            this._firstDependency = firstDependency;
        }

        public IDependency1 FirstDependency
        {
            get 
            { 
                return this._firstDependency; 
            }
            private set 
            {
                this.numberOfCallsToFirstDependencySetter += 1;
                this._firstDependency = value; 
            }
        }

        public IDependency2 SecondDependency 
        {
            get 
            { 
                return this._secondDependency; 
            }
            set
            {
                this.numberOfCallsToSecondDependencySetter += 1;
                this._secondDependency = value;
            }
        }

        public IDependency1 ThirdDependency
        {
            get
            {
                return this._thirdDependency;
            }
            set
            {
                this.numberOfCallsToThirdDependencySetter += 1;
                this._thirdDependency = value;
            }
        }

        private IDependency1 _firstDependency;
        private IDependency2 _secondDependency;
        private IDependency1 _thirdDependency;

        #region "public "
        public List<object> constructorArgsUsed = new List<object>();
        public int numberOfCallsToFirstDependencySetter = 0;
        public int numberOfCallsToSecondDependencySetter = 0;
        public int numberOfCallsToThirdDependencySetter = 0;
        #endregion
    }
}
