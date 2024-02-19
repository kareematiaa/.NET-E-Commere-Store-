
using amazon.APIs.Helpers;
using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Repository;
using Amazon.Repository.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amazon.APIs.Extentions;
using StackExchange.Redis;
using Amazon.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Amazon.Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using amazon.APIs.Errors;
using amazon.APIs.Middlewares;

namespace amazon.APIs

{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region configure services 

            // Add services to the container.

            builder.Services.AddControllers();


            builder.Services.AddSwaggerServices();

            builder.Services.AddIdentityServices(builder.Configuration);


            //database
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connnection = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));

                return ConnectionMultiplexer.Connect(connnection);
            });



            //AppServicesExtension.AddAppServices(builder.Services);

            builder.Services.AddAppServices();


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {


                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                                  .SelectMany(p => p.Value.Errors)
                                                                  .Select(E => E.ErrorMessage)
                                                                  .ToList();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            //builder.Services.AddCors();

            #endregion

            var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>(); // ask explicilty
                await dbContext.Database.MigrateAsync(); //apply migration

                await StoreContextSeed.SeedAsync(dbContext);

                var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityContext.Database.MigrateAsync();

                var userManger = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManger);


            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error accured during apply the migration");

            }


            #region Configure kestrel

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
              app.UseSwaggerMiddlewares();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors((e) => e.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}