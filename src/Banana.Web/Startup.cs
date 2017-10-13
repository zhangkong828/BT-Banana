using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Banana.Web.Middleware;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using StackExchange.Redis;
using Banana.Web.Services;
using System.Text;

namespace Banana.Web
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
            //services.AddScoped<IRedisService, RedisService>();
            //services.AddScoped<IElasticSearchService, ElasticSearchService>();

            services.AddMemoryCache();

            services.AddMvc();

            #region Redis
            //var connectionMultiplexer = ConnectionMultiplexer.Connect(Configuration["Redis:Connection"]);
            //var RedisDatabase = connectionMultiplexer.GetDatabase(0);
            //services.AddScoped(_ => RedisDatabase);
            #endregion


            #region ElasticSearch
            //var EsUrls = Configuration["ElasticSearch:Url"].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            //var EsNodes = new List<Uri>();
            //EsUrls.ForEach(url =>
            //{
            //    EsNodes.Add(new Uri(url));
            //});
            //var EsPool = new Elasticsearch.Net.StaticConnectionPool(EsNodes);
            //var EsSettings = new Nest.ConnectionSettings(EsPool);
            //var EsClient = new Nest.ElasticClient(EsSettings);
            //services.AddScoped(_ => EsClient);
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
