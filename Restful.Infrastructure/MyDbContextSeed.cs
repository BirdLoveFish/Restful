using Microsoft.Extensions.Logging;
using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Core
{
    public class MyDbContextSeed
    {
        public static void Seed(MyContext context, ILoggerFactory loggerFactory,
            int retry = 0)
        {
            int retryForAvailablity = retry;
            try
            {
                context.Database.EnsureCreated();
                if (!context.Countries.Any())
                {
                    context.Countries.AddRange(new List<Country>
                    {
                        new Country
                        {
                            Id = Guid.Parse("9b342c31-b597-40f3-af62-42fc661479c1"),
                            ChineseName = "中国",
                            EnglishName = "Chinese",
                            Abbreviation = "CN",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("9b342c31-b597-40f3-af62-42fc661479c1"),
                                    Name = "上海",
                                },
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("9b342c31-b597-40f3-af62-42fc661479c1"),
                                    Name = "北京",
                                },
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("9b342c31-b597-40f3-af62-42fc661479c1"),
                                    Name = "广州",
                                },
                            }
                        },
                        new Country
                        {
                            Id = Guid.Parse("9d5e6e8a-2501-4a2c-9473-464d33d99bd0"),
                            ChineseName = "美国",
                            EnglishName = "America",
                            Abbreviation = "USA",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("9d5e6e8a-2501-4a2c-9473-464d33d99bd0"),
                                    Name = "洛杉矶",
                                },
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("9d5e6e8a-2501-4a2c-9473-464d33d99bd0"),
                                    Name = "纽约",
                                },
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("9d5e6e8a-2501-4a2c-9473-464d33d99bd0"),
                                    Name = "芝加哥",
                                },
                            }
                        },
                        new Country
                        {
                            Id = Guid.Parse("80ee27db-301b-414c-abb2-0555b331dc00"),
                            ChineseName = "日本",
                            EnglishName = "Japanese",
                            Abbreviation = "JP",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("80ee27db-301b-414c-abb2-0555b331dc00"),
                                    Name = "东京",
                                },
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("80ee27db-301b-414c-abb2-0555b331dc00"),
                                    Name = "北海道",
                                },
                                new City
                                {
                                    Id = Guid.NewGuid(),
                                    CountryId = Guid.Parse("80ee27db-301b-414c-abb2-0555b331dc00"),
                                    Name = "名古屋",
                                },
                            }
                        },
                    });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if(retryForAvailablity < 10)
                {
                    retryForAvailablity++;
                    var logger = loggerFactory.CreateLogger<MyDbContextSeed>();
                    logger.LogError(ex.Message);
                    Seed(context, loggerFactory, retryForAvailablity);
                }
            }
        }
    }
}
