#pragma warning disable 1591
namespace SearchSuggestions.WebAPI
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Middleware;
    using SearchEngine;
    using Serilog;
    using Swashbuckle.AspNetCore.Swagger;
    using Types;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Description = "Rating API", Title = "API", Version = "v1"});
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.DescribeStringEnumsInCamelCase();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            services.AddMemoryCache();

            services.AddScoped<IIndexedSearchDataRepository, IndexedSearchDataRepository>();
            services.AddScoped<ISearchDataRepository, SearchDataRepository>();
            services.AddScoped<IScorer<string>, CityNameMatchScorer>();
            services.AddScoped<IScorer<LocationInformation>, LocationMatchScorer>();
            services.AddScoped<CitySearchEngine>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseMvc();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            // Add Serilog to the .NET Core main logging pipeline
            loggerFactory.AddSerilog();

            // Initialize databases asynchronously
            Task.Run(() => DataInitializer.InitializeData(cache));
        }
    }
}
#pragma warning restore 1591