using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XDbAccess.AutoTrans;
using XDbAccess.MSSql;
using XDbAccess.MySql;
using XDbAccess.Dapper;
using XDbAccess.Demo.Repositories;

namespace XDbAccess.Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Env = env;

            Configuration = configuration;

            LoggerFactory = loggerFactory;
        }

        public IHostingEnvironment Env { get; }

        public IConfiguration Configuration { get; }

        public ILoggerFactory LoggerFactory { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            if (Env.EnvironmentName == "MySQL")
            {
                services.AddDbContext<DapperTestDbContext>((options) =>
                {
                    options.UseMySql(Configuration.GetConnectionString("DefaultConnection"))
                        .UseLoggerFactory(LoggerFactory);
                })
                .AddMySqlDbHepler<DapperTestDbContext>()
                .AddDbContext<DapperTest2DbContext>((options) =>
                {
                    options.UseMySql(Configuration.GetConnectionString("Connection2"))
                        .UseLoggerFactory(LoggerFactory);
                })
                .AddMySqlDbHepler<DapperTest2DbContext>()
                .AddMySqlRepositories();
            }
            else
            {
                services.AddDbContext<DapperTestDbContext>((options) =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                        .UseLoggerFactory(LoggerFactory);
                })
                .AddMSSqlDbHepler<DapperTestDbContext>()
                .AddDbContext<DapperTest2DbContext>((options) =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Connection2"))
                        .UseLoggerFactory(LoggerFactory);
                })
                .AddMSSqlDbHepler<DapperTest2DbContext>()
                .AddRepositories();
            }

            services.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
