global using MediatR;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace S3.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(x=> x.RegisterServicesFromAssembly(thisAssembly));

        services.AddAWSService<IAmazonS3>();

        return services;
    }
}
