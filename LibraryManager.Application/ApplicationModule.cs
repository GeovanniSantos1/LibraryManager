using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManager.Application.Commands.Books.InsertBook;
using LibraryManager.Application.Models;
using LibraryManager.Application.Services;
using LibraryManager.Application.Validators.Books;
using LibraryManager.Application.Validators.Loans;
using LibraryManager.Application.Validators.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManager.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddServices()
                .AddHandleres()
                .AddValidation();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();

            return services;
        }

        private static IServiceCollection AddHandleres(this IServiceCollection services)
        {
            services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<InsertBookCommand>());

            services.AddTransient<IPipelineBehavior<InsertBookCommand, ResultViewModel<int>>, ValidateInsertBookCommandBehavior>();

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<InsertBookValidator>()
                .AddValidatorsFromAssemblyContaining<UpdateBookValidator>()
                .AddValidatorsFromAssemblyContaining<DeleteBookValidator>();

            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<InsertUserValidator>()
                .AddValidatorsFromAssemblyContaining<UpdateUserValidator>()
                .AddValidatorsFromAssemblyContaining<DeleteUserValidator>();

            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<InsertLoanValidator>()
                .AddValidatorsFromAssemblyContaining<LoanReturnValidator>()
                .AddValidatorsFromAssemblyContaining<DeleteLoanValidator>();

            return services;
        }
    }
}
