using ApiDataCollection.Helpers;
using ApiDataCollection.Models;
using Xunit;

namespace ApiDataCollection.Tests
{
    public class TestCountryFilter
    {
        #region Name filtering
        [Fact]
        public void Test_GetCountryFilteredByName_NoFilter()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { Name = "USA" },
                new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetCountryFilteredByName(countries, null);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Test_GetCountryFilteredByName_FilterApplied()
        {
            // Arrange
            var countries = new List<Country>
            {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetCountryFilteredByName(countries, "Canada");

            // Assert
            Assert.Single(result);
            Assert.Equal("Canada", result.First().Name);
        }

        [Fact]
        public void Test_GetCountryFilteredByName_NoMatches()
        {
            // Arrange
            var countries = new List<Country>
            {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetCountryFilteredByName(countries, "Germany");

            // Assert
            Assert.Empty(result);
        }
        #endregion

        #region Population limit
        [Fact]
        public void Test_GetCountriesWithPopulationLessThanLimit_NoLimit()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA", Population = 331002651 },
            new Country { Name = "Canada", Population = 37742154 },
            new Country { Name = "Mexico", Population = 128932753 }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetCountriesWithPopulationLessThanLimit(countries, null);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Test_GetCountriesWithPopulationLessThanLimit_LimitApplied()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA", Population = 331002651 },
            new Country { Name = "Canada", Population = 37742154 },
            new Country { Name = "Mexico", Population = 128932753 }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetCountriesWithPopulationLessThanLimit(countries, 100000000);

            // Assert
            Assert.Single(result);
            Assert.Equal("Canada", result.First().Name);
        }

        [Fact]
        public void Test_GetCountriesWithPopulationLessThanLimit_NoMatches()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA", Population = 331002651 },
            new Country { Name = "Canada", Population = 37742154 },
            new Country { Name = "Mexico", Population = 128932753 }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetCountriesWithPopulationLessThanLimit(countries, 20000000);

            // Assert
            Assert.Empty(result);
        }
        #endregion

        #region Sorting
        [Fact]
        public void Test_GetOrderedCountriesList_NoSorting()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetOrderedCountriesList(countries, null);

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("USA", result.First().Name);
        }

        [Fact]
        public void Test_GetOrderedCountriesList_AscSorting()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetOrderedCountriesList(countries, "Asc");

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("Canada", result.First().Name);
        }

        [Fact]
        public void Test_GetOrderedCountriesList_DescSorting()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetOrderedCountriesList(countries, "Desc");

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("USA", result.First().Name);
        }
        #endregion

        #region Pagination
        [Fact]
        public void Test_GetFirstCountriesFromList_NoCount()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetFirstCountriesFromList(countries, null);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Test_GetFirstCountriesFromList_CountLessThanListLength()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetFirstCountriesFromList(countries, 2);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("USA", result.First().Name);
        }

        [Fact]
        public void Test_GetFirstCountriesFromList_CountGreaterThanListLength()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = "USA" },
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" }
        };
            var service = new CountriesResponseHandlers(); // Replace with your class name

            // Act
            var result = service.GetFirstCountriesFromList(countries, 5);

            // Assert
            Assert.Equal(3, result.Count());
        }
        #endregion
    }
}