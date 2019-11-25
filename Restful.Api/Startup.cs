using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Restful.Api.Configurations;
using Restful.Core.Services;
using Restful.Core;
using Restful.Core.Repositories;
using Restful.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;
using AspNetCore.IServiceCollection.AddIUrlHelper;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Restful.Infrastructure.Resourses;
using Restful.Api.Validators;
using Restful.Api.Extensions;

namespace Restful.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllers()
                .AddFluentValidation()
                .AddNewtonsoftJson(options=>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });




            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.Audience = "api1";
                });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            services.AddUrlHelper();
            services.AddDbContext<MyContext>(options => 
                options.UseSqlite("Data Source=Restful.db"));
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddTransient<IValidator<CountryAddViewModel>, CountryAddViewModelValidator<CountryAddViewModel>>();
            services.AddTransient<IValidator<CountryUpdateViewModel>, CountryAddViewModelValidator<CountryUpdateViewModel>>();
            services.AddTestContainer();
            services.AddPropertyMapping();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(new CustomExceptionHandler(loggerFactory));
           
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
