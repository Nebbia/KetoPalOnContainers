using FluentAssertions;
using KetoPal.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KetoPal.Tests
{
    [TestClass]
    public class UserUnitTests
    {
        [TestMethod]
        public void RecordConsumption_NoPreviousConsumption_IsAdded()
        {
            //Arrange
            var user = new User();

            //Act
            user.RecordConsumption(1.1);

            //Assert
            user.TotalCarbConsumption.Should().Be(1.1);
        }

        [TestMethod]
        public void RecordConsumption_HasPreiousConsumption_TotalsIsSum()
        {
            //Arrange
            var user = new User();

            //Act
            user.RecordConsumption(1.1);
            user.RecordConsumption(5);

            //Assert
            user.TotalCarbConsumption.Should().Be(6.1);
        }

        [TestMethod]
        public void CanConsumeMore_NoPreviousConsumptionTriesToConsume5CarbsWithPreference10_ReturnsTrue()
        {
            //Arrange
            var maxCarbs = 10;
            var user = new User()
            {
                Preference = new Preference()
                {
                    MaxCarbsPerDayInGrams = maxCarbs
                }
            };
            var product = new Product()
            {
                Carbs = 5
            };

            //Act
            var canConsume = user.CanConsume(product);

            //Assert
            canConsume.Should().BeTrue();
        }

        [TestMethod]
        public void CanConsumeMore_HasEatenSomethingAndTriesToExceedLimit_ReturnsFalse()
        {
            //Arrange
            var maxCarbs = 10;
            var productCarbs = 5;
            var user = new User()
            {
                Preference = new Preference()
                {
                    MaxCarbsPerDayInGrams = maxCarbs
                }
            };
            var product = new Product()
            {
                Carbs = productCarbs
            };

            //Act
            user.RecordConsumption(product.Carbs); // + 5
            user.RecordConsumption(product.Carbs); // + 5
            var canConsume = user.CanConsume(product);

            //Assert
            canConsume.Should().BeFalse();
        }
    }
}