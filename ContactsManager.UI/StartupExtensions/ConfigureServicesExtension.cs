using ContactsManager.Core.Domain.IdentityEntities;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CRUDExample
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices (this IServiceCollection services , 
            IConfiguration configuration)
        {
            services.AddTransient<ResponsHeaderActionFilter>();
            //it adds controllers and views as services 
            services.AddControllersWithViews(options =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponsHeaderActionFilter>>();
                
                options.Filters.Add(new ResponsHeaderActionFilter(logger)
                {
                    Key = "My-Global-Custom-Key",
                    Value = "My-Global-Custom-Value",
                    Order = 2
                });
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });


            //Add service into IoC container
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddTransient<PersonsListActionFilter>();

            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();
            services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>(); 
            
          

            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //Enable identity in this project 
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3;  //Eg: AB12AB the unique here are 4 chars : a , b , 1 , 2 
            }).
                AddEntityFrameworkStores<ApplicationDbContext>().

                AddDefaultTokenProviders().
                AddUserStore<UserStore<ApplicationUser, ApplicationRole, 
                ApplicationDbContext, Guid>>().

                AddRoleStore<RoleStore<ApplicationRole,
                ApplicationDbContext, Guid>>(); 

            services.AddAuthorization(options =>{
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //enforces authorization policy (user must be  authenticated) for all the action methods
                options.AddPolicy("NotAuthorized" , policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity.IsAuthenticated;
                    });
                });
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LogoutPath = "/Account/Login"; 
            });
            // HTTP logging is enabled by calling AddHttpLogging and UseHttpLogging,
            services.AddHttpLogging(Opitions =>
            {
                Opitions.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
                Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services; 
        } 
    }
}
