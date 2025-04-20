﻿using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Services;
using Shared.ErrorModels;
using Store.API.Middlewares;

namespace Store.API.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerServices();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();

            services.ConfigureServices();
            return services;
        }
        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                              .Select(m => new ValidationError()
                                              {
                                                  Field = m.Key,
                                                  Errors = m.Value.Errors.Select(e => e.ErrorMessage)
                                              });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }
        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {

            await app.InitializeDatabaseAsync();
            #region Seeding

            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR Create Object from DbInitializer
            await dbInitializer.InitializeAsync(); // Call InitializeAsync Method

            #endregion

            app.UseGlobalErrorHandlingMiddleware();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            return app;
        }
        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {


            #region Seeding

            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR Create Object from DbInitializer
            await dbInitializer.InitializeAsync(); // Call InitializeAsync Method

            #endregion
            return app;
        }
        private static WebApplication UseGlobalErrorHandlingMiddleware(this WebApplication app)
        {

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }
    }
}
