// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZM;
using System.Reflection;

static void ConfigureServices(IServiceCollection services)
{
    services.AddLogging(configure => configure.AddConsole());
    
    // Build configuration
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true)
        .Build();

    // Add access to generic IConfigurationRoot
    services.AddSingleton<IConfigurationRoot>(configuration);

    services.AddTransient<Codegen>();
}

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);

var serviceProvider = serviceCollection.BuildServiceProvider();

Codegen? codegen = serviceProvider.GetService<Codegen>();
if (codegen != null)
{
    Assembly assembly = Assembly.Load("ZM.Examples");
    codegen.Generate(assembly);
}
