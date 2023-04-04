using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YouTubeV2.Api.Middleware;
using YouTubeV2.Application;
using YouTubeV2.Application.Configurations;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services;
using YouTubeV2.Application.Services.AzureServices.BlobServices;
using YouTubeV2.Application.Services.JwtFeatures;
using YouTubeV2.Application.Validator;
using Microsoft.OpenApi.Models;

public partial class Program {
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        AddServices(builder);

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("Allow ALL");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Add JWT authentication to Swagger UI
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };
            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                { securityScheme, new[] { "Bearer" } }
            };
            c.AddSecurityRequirement(securityRequirement);
        });

        builder.Services.AddOptions<BlobStorageConfig>().Bind(builder.Configuration.GetSection("BlobStorage"));

        string connectionString = builder.Configuration.GetConnectionString("Db")!;
        builder.Services.AddDbContext<YTContext>(
            options => options.UseSqlServer(connectionString));


        builder.Services.AddTransient<UserService>();
        builder.Services.AddTransient<SubscriptionsService>();
        builder.Services.AddSingleton(x => new BlobServiceClient(Environment.GetEnvironmentVariable("AZURE_IMAGES_BLOB_STORAGE_CONNECTION_STRING")));
        builder.Services.AddSingleton<IBlobImageService, BlobImageService>();


        builder.Services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

        builder.Services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<YTContext>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "Allow ALL",
                policyBuilder => policyBuilder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .WithExposedHeaders("Content-Disposition"));
        });

        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
               options.TokenLifespan = TimeSpan.FromHours(2));

        var jwtSettings = new JwtSettings(builder.Configuration.GetSection("JWTSettings"));
        builder.Services.AddSingleton(jwtSettings);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.SecurityKey)
            };
        });
        builder.Services.AddScoped<JwtHandler>();
    }
}