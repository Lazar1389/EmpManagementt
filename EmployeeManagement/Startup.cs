using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement
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
           // services.AddEntityFrameworkSqlServer();
            
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeDBConnection")) );
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequiredLength=10;
                options.Password.RequiredUniqueChars=3;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppDbContext>();




            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }); 

            services.AddAuthentication()
                .AddGoogle(options => { 
                options.ClientId = "187918160914-5shslkno0f26n3i529hp5rrfios20e86.apps.googleusercontent.com";
                options.ClientSecret = "j-_ruXhpKwyg-98Yrnuc9McQ";
                    }
                ).AddFacebook(options =>
                {
                    options.AppId = "3021908864700056";
                    options.AppSecret = "a0cae985812451e9cc69fcca809b30e2";


                });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Administration/AccessDenied");
            });
             services.AddAuthorization(options =>
                {
                    options.AddPolicy("DeleteRolePolicy", 
                    policy => policy.RequireClaim("Delete Role"));

                      options.AddPolicy("EditRolePolicy", 
                    policy => policy.RequireAssertion(context=>
                    context.User.IsInRole("Admin")&&
                    context.User.HasClaim(claim => claim.Type=="EditRole" && claim.Value=="true") ||
                    context.User.IsInRole("Super admin")
                    ));

                      options.AddPolicy("AdminRolePolicy", 
                    policy => policy.RequireClaim("Admin"));
                }); 
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
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
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
