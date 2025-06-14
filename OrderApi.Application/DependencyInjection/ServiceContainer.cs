using eCommerce.SharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;

namespace OrderApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IOrderService, OrderService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiGateway: BaseAddress"]!);
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            
            var retryPolicy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                MaxRetryAttempts = 3,
                UseJitter = true,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = arg =>
                {
                    string message = $"Retry attempt {arg.AttemptNumber} : Out Come {arg.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }   
            };
            
            services.AddResiliencePipeline("my-retry-pipeline", options => options.AddRetry(retryPolicy));
            return services;
        }
    }
}
