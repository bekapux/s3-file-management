global using Amazon.S3;
global using Amazon.S3.Model;
global using Microsoft.Extensions.Configuration;
global using S3.Application.Features.Files.Models;
global using S3.Application.Interfaces;
using Amazon;
using Microsoft.Extensions.DependencyInjection;
using S3.Infrastructure.Services;

namespace S3.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureAWSS3(configuration);

        services.AddScoped<IFileService, S3Service>();

        return services;
    }

    private static void ConfigureAWSS3(this IServiceCollection services, IConfiguration configuration)
    {
        var accessKey = configuration.GetSection("S3Service:AccessKey").Value;
        var roleSecret = configuration.GetSection("S3Service:RoleSecret").Value;

        var awsOptions = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.EUNorth1,
        };
        services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(accessKey, roleSecret, awsOptions));
    }
}
