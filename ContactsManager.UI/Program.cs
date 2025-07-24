using CRUDExample;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using CRUDExample.Middleware;


var builder = WebApplication.CreateBuilder(args);


//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services,
    LoggerConfiguration loggerConfiguration) =>
{

    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} else
{
    app.UseExceptionHandler("/Error"); 
    app.UseExceptionHandlingMiddleware();
}

app.UseHsts();
app.UseHttpsRedirection();

    //Http Logs
app.UseHttpLogging();

if (builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");


app.UseStaticFiles();


app.UseRouting();//Identifying action method based on route 
app.UseAuthentication();//Reading Identity cookie route
app.UseAuthorization();//Validates access permissions of the user 
app.MapControllers();//Exectue the filter pipiline (action + filters )  

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllerRoute(
        name: "Admin",
       pattern: "{area:exists}/{controller=Home}/{action=Index}"
      );
});

app.Run();

public partial class Program { } //make the auto-generated Program accessible programmatically


