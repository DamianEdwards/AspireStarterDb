using Microsoft.Extensions.DependencyInjection;

namespace AspireStarterDb.AppHost;

internal static class ResourceBuilderExtensions
{
#pragma warning disable ASPIREEVENTING001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    /// <summary>
    /// Adds a <see cref="HealthChecks.Uris.UriHealthCheck"/> to the resource that checks the endpoint named 'https'.
    /// </summary>
    public static IResourceBuilder<TResource> WithHttpsHealthCheck<TResource>(this IResourceBuilder<TResource> builder, string? path = null)
        where TResource : IResourceWithEndpoints
        => WithUriHealthCheck(builder, "https", path);

    /// <summary>
    /// Adds a <see cref="HealthChecks.Uris.UriHealthCheck"/> to the resource that checks the endpoint named 'http'.
    /// </summary>
    public static IResourceBuilder<TResource> WithHttpHealthCheck<TResource>(this IResourceBuilder<TResource> builder, string? path = null)
        where TResource : IResourceWithEndpoints
        => WithUriHealthCheck(builder, "http", path);

    /// <summary>
    /// Adds a <see cref="HealthChecks.Uris.UriHealthCheck"/> to the resource that checks the endpoint with the specified name.
    /// </summary>
    /// <remarks>
    /// Only HTTP-based endpoints are supported. The endpoint must have a scheme of 'http' or 'https'.
    /// </remarks>
    public static IResourceBuilder<TResource> WithUriHealthCheck<TResource>(this IResourceBuilder<TResource> builder, string endpointName, string? path = null)
        where TResource : IResourceWithEndpoints
    {
        var endpoints = builder.Resource.GetEndpoints();
        var endpoint = endpoints.FirstOrDefault(e => string.Equals(e.EndpointName, endpointName, StringComparison.OrdinalIgnoreCase))
            ?? throw new DistributedApplicationException($"Could not add URI health check for resource '{builder.Resource.Name}' as no endpoint named '{endpointName}' was found.");

        var healthCheckName = string.IsNullOrWhiteSpace(path)
            ? $"{builder.Resource.Name} {endpointName} health check"
            : $"{builder.Resource.Name} {endpointName} {path} health check";

        builder.WithHealthCheck(healthCheckName);

        Uri? uri = null;

        var sub = builder.ApplicationBuilder.Eventing.Subscribe<AfterEndpointsAllocatedEvent>((e, ct) =>
        {
            if (!string.Equals(endpoint.Scheme, "https", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(endpoint.Scheme, "http", StringComparison.OrdinalIgnoreCase))
            {
                throw new DistributedApplicationException(
                    $"Could not add URI health check for endpoint named '{endpointName}' on resource '{builder.Resource.Name}'. Only HTTP-based endpoints are supported.");
            }

            var uriBuilder = new UriBuilder(endpoint.Url);

            if (!string.IsNullOrWhiteSpace(path))
            {
                uriBuilder.Path = path;
            }
            uri = uriBuilder.Uri;

            return Task.CompletedTask;
        });

        builder.ApplicationBuilder.Services.AddHealthChecks()
            .AddUrlGroup(name: healthCheckName, uriOptions: options =>
            {
                if (uri is not null)
                {
                    options.AddUri(uri);
                }
            });

        return builder;
    }

#pragma warning restore ASPIREEVENTING001
}
