using Microsoft.EntityFrameworkCore;
using Restful.Core.Models;
using Restful.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Restful.Core.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MyContext context;

        public CountryRepository(MyContext context)
        {
            this.context = context;
        }

        public async Task AddCountriesCollection(List<Country> countries)
        {
            await context.Countries.AddRangeAsync(countries);
        }

        public async Task AddCountryAsync(Country country)
        {
            //country.Id = Guid.NewGuid();
            await context.Countries.AddAsync(country);
        }

        public async Task<bool> CountryExists(Guid id)
        {
            return await context.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task DeleteCountry(Country country)
        {
            context.Countries.Remove(country);
        }

        public async Task<PagintedList<Country>> GetCountriesAsync(CountryResourceParameters parameters)
        {
            var query = context.Countries.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.EnglishName))
            {
                var englishName = parameters.EnglishName.Trim().ToLowerInvariant();
                query = query.Where(a => a.EnglishName.ToLowerInvariant() == englishName);
            }
            if (!string.IsNullOrEmpty(parameters.ChineseName))
            {
                //var chineseName = parameters.ChineseName.Trim().ToLowerInvariant();
                query = query.Where(a => a.ChineseName.Equals(parameters.ChineseName));
            }

            var propertiesMap = new Dictionary<string, Expression<Func<Country, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id",c=>c.Id },
                {"ChineseName",c=>c.ChineseName },
                {"EnglishName",c=>c.EnglishName },
                {"Abbreviation",c=>c.Abbreviation },
            };

            if (!string.IsNullOrEmpty(parameters.OrderBy))
            {
                var isDesc = parameters.OrderBy.EndsWith(" desc");
                var property = isDesc ? parameters.OrderBy.Split(' ')[0] : parameters.OrderBy;
                query = query.OrderBy(property + (isDesc ? " desc" : " asc"));
            }
            
            var count = await query.CountAsync();
            var list = await query
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagintedList<Country>(parameters.PageSize, parameters.PageIndex,count, list);
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync(IEnumerable<Guid> ids)
        {
            return await context.Countries.Where(c=>ids.Contains(c.Id)).ToListAsync();
        }

        public async Task<Country> GetCountryById(Guid id, bool includeCities = false)
        {
            if(includeCities)
                return await context.Countries.Include(a => a.Cities)
                    .SingleOrDefaultAsync(a => a.Id == id);
            return await context.Countries.FindAsync(id);
        }

        public async Task UpdateCountry(Country country)
        {
            context.Countries.Update(country);
        }
    }
}
