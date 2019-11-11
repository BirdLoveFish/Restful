using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restful.Core.Models;

namespace Restful.Core.Services
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetCountriesAsync();

        Task AddCountryAsync(Country country);

        Task<Country> GetCountryById(Guid id);

        Task<bool> CountryExists(Guid id);
    }
}