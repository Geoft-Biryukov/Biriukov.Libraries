using Biriukov.OrdinaryDifferentialEquations;

namespace OrdinaryDifferentialEquations.Tests
{
    [TestFixture]
    public class Tests
    {        
        #region Constructors
        [Test]
        public void CanCreateStateVector()
        {
            //Arrange
            int n = 5;
            // Apply
            var v = new StateVector(n);
            // Assert
            Assert.That(v, Is.Not.Null);
        }

        [Test]
        public void MustThrow_CreateStateVector_WithZero_Or_Negative()
        {
            //Arrange
            int n = 0;
            int m = -5;
            // Apply            
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new StateVector(n));
            Assert.Throws<ArgumentOutOfRangeException>(() => new StateVector(m));
        }

        [Test]
        public void CanCreateStateVector_WithDoubleArray()
        {
            //Arrange
            var init = new double[] { 1, 2, 3 };
            // Apply
            var v = new StateVector(init);
            // Assert
            Assert.That(v, Is.Not.Null);
        }

        [Test]
        public void MustThrow_CreateStateVector_WithNull()
        {
            //Arrange
            double[]? init = null;
            // Apply            
            // Assert
            Assert.Throws<ArgumentNullException>(() => new StateVector(init));
        }

        [Test]
        public void MustThrow_CreateStateVector_WithEmptyArray()
        {
            //Arrange
            double[] init = [];
            // Apply
            // Assert
            Assert.Throws<ArgumentException>(() => new StateVector(init));
        }
        #endregion

        #region Indexer
        [Test]
        public void IndexerReturnsCorrectValues()
        {
            //Arrange
            var array = new double[] { 1, 2, 3 };
            // Apply
            var v = new StateVector(array);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(v[0], Is.EqualTo(array[0]));
                Assert.That(v[1], Is.EqualTo(array[1]));
                Assert.That(v[2], Is.EqualTo(array[2]));
            });
        }

        [Test]
        public void IndexerThrows_IfIndexIsOutOfRange()
        {
            //Arrange
            int n = 5;
            // Apply
            var v = new StateVector(n);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var tmp = v[-1]; });
                Assert.Throws<ArgumentOutOfRangeException>(() => { var tmp = v[5]; });                       
            });
        }
        #endregion

        #region CopyTo
        [Test]
        public void CopyToWorksCorrectly()
        {
            //Arrange
            var source = new double[] { 1, 2, 3 };
            var fullCopy = new double[3];
            var partialCopy = new double[4];
            // Apply
            var v = new StateVector(source);
            v.CopyTo(fullCopy, 0);
            v.CopyTo(partialCopy, 1);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(fullCopy, Is.EqualTo(source));
                Assert.That(partialCopy[1], Is.EqualTo(source[0]));
                Assert.That(partialCopy[2], Is.EqualTo(source[1]));
                Assert.That(partialCopy[3], Is.EqualTo(source[2]));
            });
        }

        [Test]
        public void CopyToThrowsIfNull()
        {
            //Arrange
            var source = new double[] { 1, 2, 3 };
            double[]? fullCopy = null;            
            // Apply
            var v = new StateVector(source);
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => v.CopyTo(fullCopy, 0));            
        }

        [Test]
        public void CopyToThrowsIfInvalidIndex()
        {
            //Arrange
            var source = new double[] { 1, 2, 3 };
            var fullCopy = new double[3];
            // Apply
            var v = new StateVector(source);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => v.CopyTo(fullCopy, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => v.CopyTo(fullCopy, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v.CopyTo(fullCopy, 1));
        }
        #endregion

        #region ToArray
        [Test]
        public void ToArrayWorksCorrectly()
        {
            //Arrange
            var source = new double[] { 1, 2, 3 };            
            // Apply
            var v = new StateVector(source);
            var result = v.ToArray();            
            // Assert
            Assert.That(result, Is.EqualTo(source));
            Assert.That(result, Is.Not.SameAs(source));
        }
        #endregion

        #region Operators
        [Test]
        public void MultiplyToNumberLeftWorksCorrectly()
        {
            //Arrange
            double number = 2.2;
            var array = new double[] { 1, 2, 3 };
            var v = new StateVector(array);
            // Apply
            var result = number * v;
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(number * array[0]));
                Assert.That(result[1], Is.EqualTo(number * array[1]));
                Assert.That(result[2], Is.EqualTo(number * array[2]));
            });
        }

        [Test]
        public void MultiplyToNumberRightWorksCorrectly()
        {
            //Arrange
            double number = 2.2;
            var array = new double[] { 1, 2, 3 };
            var v = new StateVector(array);
            // Apply
            var result = v * number;
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(number * array[0]));
                Assert.That(result[1], Is.EqualTo(number * array[1]));
                Assert.That(result[2], Is.EqualTo(number * array[2]));
            });
        }

        [Test]
        public void AdditionWorksCorrectly()
        {
            //Arrange            
            var array1 = new double[] { 1, 2, 3 };
            var array2 = new double[] { 3, 2, 1 };
            var v1 = new StateVector(array1);
            var v2 = new StateVector(array2);

            // Apply
            var result = v1 + v2;
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(v1[0] + v2[0]));
                Assert.That(result[1], Is.EqualTo(v1[0] + v2[0]));
                Assert.That(result[2], Is.EqualTo(v1[0] + v2[0]));
            });
        }

        [Test]
        public void AdditionThrows_MismatchOrders()
        {
            //Arrange            
            var array1 = new double[] { 1, 2, 3 };
            var array2 = new double[] { 3, 2, 1, 5 };
            var v1 = new StateVector(array1);
            var v2 = new StateVector(array2);

            // Apply


            // Assert

            Assert.Throws<ArgumentException>(() => { var res = v1 + v2; });
        }

        [Test]
        public void SubtractionWorksCorrectly()
        {
            //Arrange            
            var array1 = new double[] { 1, 2, 3 };
            var array2 = new double[] { 3, 2, 1 };
            var v1 = new StateVector(array1);
            var v2 = new StateVector(array2);

            // Apply
            var result = v1 - v2;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(v1[0] - v2[0]));
                Assert.That(result[1], Is.EqualTo(v1[1] - v2[1]));
                Assert.That(result[2], Is.EqualTo(v1[2] - v2[2]));
            });
        }

        [Test]
        public void SubtractionThrows_MismatchOrders()
        {
            //Arrange            
            var array1 = new double[] { 1, 2, 3 };
            var array2 = new double[] { 3, 2, 1, 5 };
            var v1 = new StateVector(array1);
            var v2 = new StateVector(array2);

            // Apply


            // Assert

            Assert.Throws<ArgumentException>(() => { var res = v1 - v2; });
        }
        #endregion
    }
}