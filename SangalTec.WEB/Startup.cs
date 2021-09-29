using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SangalTec.Bunsiness.Abstract;
using SangalTec.DAL;
using SangalTec.Bunsiness.Bunsiness;
using SangalTec.Models.EntitiesUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SangalTec.WEB
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
            services.AddControllersWithViews();
            //Conexion DB
            var conexion = Configuration["ConnectionStrings:SqlServer"]; //Cadena de conexion
            services.AddDbContext<SangalDbContext>(option =>
            option.UseSqlServer(conexion)
        );

            services.AddScoped<IUsuarioBunsiness, UsuarioBunsiness>();

            services.AddScoped<IProductoBunsiness, ProductoBunsiness>();

            services.AddScoped<ICategoriaBunsiness, CategoriaBunsiness>();

            services.AddScoped<IProveedorBusiness, ProveedorBusiness>();

            services.AddScoped<IInsumoBusiness, InsumoBusiness>();

            //Indentity

            services.AddIdentity<Usuario, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
              //.AddDefaultUI()
              .AddDefaultTokenProviders() //para trabajar con la confirmaci�n de email
              .AddEntityFrameworkStores<SangalDbContext>();
            //.AddClaimsPrincipalFactory<UsuarioClaimsPrincipalFactory>();

            //configuraci�n del password
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;
            });

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Usuarios}/{action=Login}/{id?}");
            });
        }
    }
}

