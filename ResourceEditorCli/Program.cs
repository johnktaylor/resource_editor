// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using ResourceEditorLib.Database;

namespace ResourceEditorCli;

public static class Program {
    
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var resourceEditorService = serviceProvider.GetService<IResourceEditorService>();
        if (resourceEditorService == null)
        {
            Console.WriteLine("No ResourceEditor Service was found.");
        }
        var returnValue = resourceEditorService?.Run(args);
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IResourceEditorService, ResourceEditorService>();
        services.AddTransient<IResourceDbContext, ResourceDbContext>();
    }
}
