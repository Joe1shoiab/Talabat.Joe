using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region ConfigureServices
            builder.Services.AddControllers();

            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAplicationServices();

            #endregion
            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {

                var context = services.GetRequiredService<StoreDbContext>();
                await context.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");
            }
            
            // Configure the HTTP request pipeline (Middlewares).
            #region Configure
            // app is an instance of WebApplication
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWare(); 
            };

            app.UseStatusCodePagesWithReExecute("/Errors/{0}"); // This will redirect to the Errors controller and pass the status code as a parameter
            app.UseHttpsRedirection();

            app.UseStaticFiles(); // This will serve static files like images, css, js, etc.
            

            app.UseAuthorization();

            //app.UseRouting(); // This will route the request to the correct controller
            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}