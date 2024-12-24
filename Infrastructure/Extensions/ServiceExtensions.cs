using System.Text;
using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Contracts.UseCasesContracts.AuthUseCasesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.UseCases.AuthorUseCases;
using Application.UseCases.AuthUseCases;
using Application.UseCases.BookUseCases;
using Application.UseCases.BorrowingUseCases;
using Application.Validation;
using Domain.Models;
using FluentValidation;
using Infrastructure.Logging;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;

namespace Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    
    public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDbContext<ApplicationContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("sqlConnection")));
    
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<User>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
            o.User.RequireUniqueEmail = true;
        });
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole),
            builder.Services);
        builder.AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration
        configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetSection("validIssuer").Value;
        services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new
                        SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };
            });
    }
    
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        //Auth use cases
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        
        //Books management use cases
        services.AddScoped<IGetBooksUseCase, GetBooksUseCase>();
        services.AddScoped<IGetBookByIdUseCase, GetBookByIdUseCase>();
        services.AddScoped<ICreateBookUseCase, CreateBookUseCase>();
        services.AddScoped<IUpdateBookUseCase, UpdateBookUseCase>();
        services.AddScoped<IDeleteBookUseCase, DeleteBookUseCase>();
        services.AddScoped<ICountBooksUseCase, CountBooksUseCase>();
        services.AddScoped<IEditBookControllerUseCase, EditBookControllerUseCase>();
        services.AddScoped<IAddBookControllerUseCase, AddBookControllerUseCase>();

        //Authors management use cases
        services.AddScoped<ICountAuthorsUseCase, CountAuthorsUseCase>();
        services.AddScoped<ICreateAuthorUseCase, CreateAuthorUseCase>();
        services.AddScoped<IDeleteAuthorUseCase, DeleteAuthorUseCase>();
        services.AddScoped<IGetAllAuthorsUseCase, GetAllAuthorsUseCase>();
        services.AddScoped<IGetAuthorByIdUseCase, GetAuthorByIdUseCase>();
        services.AddScoped<IUpdateAuthorUseCase, UpdateAuthorUseCase>();
        
        //Borrowing of books use cases
        services.AddScoped<ICountBorrowsUseCase, CountBorrowsUseCase>();
        services.AddScoped<ICreateBorrowUseCase, CreateBorrowUseCase>();
        services.AddScoped<IGetUserBorrowUseCase, GetUserBorrowUseCase>();
        services.AddScoped<IGetUsersBorowsUseCase, GetUsersBorowsUseCase>();
        services.AddScoped<IBookInfoControllerUseCase, BookInfoControllerUseCase>();
        services.AddScoped<ITakeBookControllerUseCase, TakeBookControllerUseCase>();
        services.AddScoped<IReturnBookUseCase, ReturnBookUseCase>();
        
        //Validation use cases
        services.AddValidatorsFromAssemblyContaining<BookValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthorValidator>();
        services.AddValidatorsFromAssemblyContaining<BorrowingValidator>();
        
        return services;
    }
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1"
            });
        s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Place to add JWT with Bearer",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        s.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Name = "Bearer",
                },
                new List<string>()
            }
        });
        });
    }
    
    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();
    
    public static void AddAuthorizationPolicy(this IServiceCollection services) =>
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                policy.RequireRole("Administrator")); 
            options.AddPolicy("User", policy =>
                policy.RequireRole("User")); 
            options.AddPolicy("AdminOrUser", policy =>
                policy.RequireRole("Administrator", "User"));
        });
}
