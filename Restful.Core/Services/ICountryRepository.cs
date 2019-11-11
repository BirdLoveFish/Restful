using System.Collections.Generic;
using System.Threading.Tasks;
using Restful.Core.Models;

namespace Restful.Core.Services
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetCountriesAync();
    }
}