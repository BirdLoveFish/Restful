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
                            Id = Guid.NewGuid(),
                            ChineseName = "中国",
                            EnglishName = "Chinese",
                            Abbreviation = "CN"
                        },
                        new Country
                        {
                            Id = Guid.NewGuid(),
                            ChineseName = "美国",
                            EnglishName = "America",
                            Abbreviation = "USA"
                        },
                        new Country
                        {
                            Id = Guid.NewGuid(),
                            ChineseName = "日本",
                            EnglishName = "Japanese",
                            Abbreviation = "JP"
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
