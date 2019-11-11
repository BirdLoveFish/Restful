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

        public async Task<IEnumerable<Country>> GetCountriesAync()
        {
            return await context.Countries.ToListAsync();
        }
    }
}
