using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;
using MyApp.Application.RepositoryCollections;
using MyApp.Application.Services;
using MyApp.Application.Validators;
using MyApp.Domain.Interfaces;

namespace MyApp.Application.ServiceCollections
{
    public static class ServiceCollections
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();

            services.AddScoped<IValidator<ProductDto>, ProductValidator>();
            services.AddScoped<IValidator<CategoryDto>, CategoryValidator>();
            services.AddScoped<IValidator<SupplierDto>, SupplierValidator>();
            services.AddScoped<IValidator<TransactionDto>, TransactionValidator>();

            services.ConfigureRepositories(configuration);
        }
    }
}
