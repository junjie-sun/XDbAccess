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
using Microsoft.Extensions.Hosting;

namespace XDbAccess.Demo
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Env = env;

            Configuration = configuration;
        }

        public IWebHostEnvironment Env { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options=>
            {
                options.EnableEndpointRouting = false;
            });

            if (Env.EnvironmentName == "MySQL")
            {
                services.AddDbContext<DapperTestDbContext>((options) =>
                {
                    options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
                })
                .AddMySqlDbHepler<DapperTestDbContext>()
                .AddDbContext<DapperTest2DbContext>((options) =>
                {
                    options.UseMySql(Configuration.GetConnectionString("Connection2"));
                })
                .AddMySqlDbHepler<DapperTest2DbContext>()
                .AddMySqlRepositories();
            }
            else
            {
                services.AddDbContext<DapperTestDbContext>((options) =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                })
                .AddMSSqlDbHepler<DapperTestDbContext>()
                .AddDbContext<DapperTest2DbContext>((options) =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Connection2"));
                })
                .AddMSSqlDbHepler<DapperTest2DbContext>()
                .AddRepositories();
            }

            services.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
