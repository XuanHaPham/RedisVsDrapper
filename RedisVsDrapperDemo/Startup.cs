using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RedisVsDapperDemo.Helper;
using RedisVsDrapperDemo.Model.Repository;
using RedisVsDrapperDemo.Model.Service;
using RedisVsDrapperDemo.Repository;
using RedisVsDrapperDemo.Service;
using Swashbuckle.AspNetCore.Swagger;

namespace RedisVsDrapperDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API"
                });
            });
            var builder = new ContainerBuilder();
            builder.Populate(services);

           
            var iServiceType = typeof(IService);
            builder.RegisterAssemblyTypes(Assembly.Load("RedisVsDrapperDemo.Service"))
                      .Where(t => iServiceType.IsAssignableFrom(t))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            var iRepositoryType = typeof(IRepository);
            builder.RegisterAssemblyTypes(Assembly.Load("RedisVsDrapperDemo.Repository"))
                      .Where(t => iRepositoryType.IsAssignableFrom(t))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.Register(context =>
                new SqlConnection(ConfigHelper.ConnectionStrings["RedisVsDapper"])
            ).As<IDbConnection>().InstancePerLifetimeScope();
            builder.Register(context =>
                UnitOfWork.Create(context.Resolve<IDbConnection>())
            ).As<IUnitOfWork>().InstancePerLifetimeScope();
            ApplicationContainer = builder.Build();
            //var dic = ApplicationContainer.Resolve<IDictionary<string, int>>();
            //var dic2 = ApplicationContainer.Resolve<Dictionary<string, int>>();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }
    }
}
/* //services.AddScoped<IUserService, UserService>()
            //    .AddScoped<IUserRepository, UserRepository>()
            //    .AddScoped<IRedisRepository, RedisRepository>();

            //builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            
            //builder.RegisterType<RedisRepository>().As<IRedisRepository>().InstancePerRequest();

            //builder.Register(context => {
            //    var result = new Dictionary<string, int>
            //    {
            //        ["3"] = 23123,
            //        ["2"] = 231563,
            //        ["1"] = 232313,
            //        ["5"] = 2313,
            //        ["6"] = 2324413
            //    };
            //    return result;
            //}).As<Dictionary<string, int>>().As<IDictionary<string,int>>().SingleInstance();
*/
