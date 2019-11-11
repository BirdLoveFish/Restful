using Microsoft.EntityFrameworkCore;
using Restful.Core;
using Restful.Core.Models;
using Restful.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restful.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly MyContext context;

        public CityRepository(MyContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<City>> GetCitiesAsync(Guid countryId)
        {
            return await context.Cities.Where(c=>c.CountryId == countryId).ToListAsync();
        }

        public async Task<City> GetCityForCountry(Guid countryId, Guid cityId)
        {
            return await context.Cities
                .Where(c=>c.CountryId == countryId & c.Id == cityId)
                .SingleOrDefaultAsync();
        }
    }
}
