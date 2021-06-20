using HolidayOptimizer.API.Filters;
using HolidayOptimizer.API.ServiceCollectionExtensions;
using HolidayOptimizer.API.Services;
using HolidayOptimizer.API.Services.Implementations;
using HolidayOptimizer.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO.Compression;

namespace HolidayOptimizer.API
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

            var nagerApiSettings = Configuration.GetSection("NaggerApiSettings").Get<NagerApiSettings>();
            services.AddSingleton(nagerApiSettings);

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appSettings);

            services.AddLogging(ctx => ctx.AddConsole());

            services.AddControllers(o =>
            {
                o.InputFormatters.RemoveType<XmlDataContractSerializerInputFormatter>();
                o.InputFormatters.RemoveType<XmlSerializerInputFormatter>();
                o.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                o.OutputFormatters.RemoveType<StreamOutputFormatter>();
                o.OutputFormatters.RemoveType<StringOutputFormatter>();
                o.OutputFormatters.RemoveType<XmlDataContractSerializerOutputFormatter>();
                o.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();

                o.Filters.Add<GlobalExceptionFilter>();
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression();

            //TODO: If needed, distributed cache can be implemented.
            services.AddMemoryCache(o =>
            {
                //Added for In-Memory cache, so it doesn't grow forever
                o.SizeLimit = appSettings.CacheSizeLimit;
            });

            services.AddNagerServiceHttpClient(nagerApiSettings);
            services.AddSingleton<IHolidayOptimizerService, HolidayOptimizerService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HolidayOptimizer.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HolidayOptimizer.API v1"));
            }

            app.UseResponseCompression();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
