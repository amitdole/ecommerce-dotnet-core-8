using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger = (context, loggerConfiguration) =>
        {
            var environment = context.HostingEnvironment;
            loggerConfiguration
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", environment.ApplicationName)
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console();

            if (environment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Override("Catalog", Serilog.Events.LogEventLevel.Debug);
                loggerConfiguration.MinimumLevel.Override("Basket", Serilog.Events.LogEventLevel.Debug);
                loggerConfiguration.MinimumLevel.Override("Discount", Serilog.Events.LogEventLevel.Debug);
                loggerConfiguration.MinimumLevel.Override("Ordering", Serilog.Events.LogEventLevel.Debug);
            }

            //Elastic Search
            var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

            if (!string.IsNullOrEmpty(elasticUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        IndexFormat = "ecommerce-Logs-{0:yyyy.MM.dd}",
                        MinimumLogEventLevel = Serilog.Events.LogEventLevel.Debug
                    });
            }
        };
    }
}