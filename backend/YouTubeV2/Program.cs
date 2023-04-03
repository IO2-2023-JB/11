using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using YouTubeV2.Api.Middleware;
using YouTubeV2.Application;
using YouTubeV2.Application.Configurations.BlobStorage;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services;
using YouTubeV2.Application.Services.AzureServices.BlobServices;
using YouTubeV2.Application.Validator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<BlobStorageImagesConfig>().Bind(builder.Configuration.GetSection("BlobStorageImagesConfig"));
builder.Services.AddOptions<BlobStorageVideosConfig>().Bind(builder.Configuration.GetSection("BlobStorageVideosConfig"));

string connectionString = builder.Configuration.GetConnectionString("Db")!;
builder.Services.AddDbContext<YTContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<UserService>();
builder.Services.AddSingleton(x => new BlobServiceClient(Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONNECTION_STRING")));
builder.Services.AddTransient<SubscriptionsService>();
builder.Services.AddSingleton<IBlobImageService, BlobImageService>();
builder.Services.AddSingleton<IBlobVideoService, BlobVideoService>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

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

if (app.Environment.IsEnvironment("Test"))
    app.MapControllers().AllowAnonymous();
else
    app.MapControllers();

app.Run();
public partial class Program { }