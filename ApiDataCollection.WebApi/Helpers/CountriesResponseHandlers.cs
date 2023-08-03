using ApiDataCollection.Models;

namespace ApiDataCollection.Helpers
{
    public class CountriesResponseHandlers
    {
        public IEnumerable<Country> GetCountryFilteredByName(IEnumerable<Country> countries, string name)
        {
            return countries.Where(c => string.IsNullOrWhiteSpace(name) || c.Name.Contains(name));
        }

        public IEnumerable<Country> GetCountriesWithPopulationLessThanLimit(IEnumerable<Country> countries, int? limit)
        {
            return countries.Where(c => limit == null || c.Population < limit);
        }

        public IEnumerable<Country> GetOrderedCountriesList(IEnumerable<Country> countries, string nameSorting)
        {
            CountryNameSorting sorting = CountryNameSorting.None;
            if (!string.IsNullOrEmpty(nameSorting) && Enum.TryParse($"{nameSorting.ToUpper()[0]}{nameSorting.Substring(1).ToLower()}", out sorting))
            {
                if (sorting == CountryNameSorting.Asc)
                {
                    countries = countries.OrderBy(c => c.Name).ToList();
                }
                else if (sorting == CountryNameSorting.Desc)
                {
                    countries = countries.OrderByDescending(c => c.Name).ToList();
                }
            }
            return countries;
        }
        public IEnumerable<Country> GetFirstCountriesFromList(IEnumerable<Country> countries, int? count)
        {
            return count.HasValue ? countries.Take(count.Value) : countries;
        }
    }
}
