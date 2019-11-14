using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restful.Core.Services
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync(Guid countryId);

        Task<City> GetCityForCountry(Guid countryId, Guid cityId);

        Task AddCityForCountry(Guid countryId, City city);
        Task DeleteCityForCountry(City city);

        Task UpdateCity(City city);
    }
}
