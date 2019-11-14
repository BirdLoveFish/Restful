using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restful.Core.Models;

namespace Restful.Core.Services
{
    public interface ICountryRepository
    {
        Task<PagintedList<Country>> GetCountriesAsync(CountryResourceParameters parameters);
        Task<IEnumerable<Country>> GetCountriesAsync(IEnumerable<Guid> ids);

        Task AddCountryAsync(Country country);

        Task AddCountriesCollection(List<Country> countries);

        Task<Country> GetCountryById(Guid id,bool includeCities = false);

        Task<bool> CountryExists(Guid id);
        Task DeleteCountry(Country country);

        Task UpdateCountry(Country country);
    }
}