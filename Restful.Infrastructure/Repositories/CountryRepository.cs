using Microsoft.EntityFrameworkCore;
using Restful.Core.Models;
using Restful.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restful.Core.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MyContext context;

        public CountryRepository(MyContext context)
        {
            this.context = context;
        }

        public async Task AddCountryAsync(Country country)
        {
            country.Id = Guid.NewGuid();
            await context.Countries.AddAsync(country);
        }

        public async Task<bool> CountryExists(Guid id)
        {
            return await context.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await context.Countries.ToListAsync();
        }

        public async Task<Country> GetCountryById(Guid id)
        {
            return await context.Countries.FindAsync(id);
        }
    }
}
