using Newtonsoft.Json.Linq;
using ResourceEditorCli.Commands;

namespace ResourceEditorCli;

public static class ResourceEditorConfiguration
{
    public static ResourceEditorResult GetSetDb()
    {
        var filePath = "appsettings.json";
        
        var json = File.ReadAllText(filePath);
        var config = JObject.Parse(json);
        
        config["Settings"] ??= new JObject();
        var setDb = config["Settings"]!["currentDb"];

        return new ResourceEditorResult()
        {
            ExitCode = 0,
            ResultMessage = setDb?.ToString()
        };
    }

    public static ResourceEditorResult SetDb(string resourceDbPath)
    {
        var filePath = "appsettings.json";
        if (File.Exists(resourceDbPath))
        {
            // Read the existing configuration file into a JObject
            var json = File.ReadAllText(filePath);
            var config = JObject.Parse(json);

            // Assuming your settings are stored under a "Settings" section, update or add a value
            var newValue = resourceDbPath;

            config["Settings"] ??= new JObject();
            config["Settings"]!["currentDb"] = newValue;

            // Write the updated JSON back to the file
            File.WriteAllText(filePath, config.ToString(Newtonsoft.Json.Formatting.Indented));

            return new ResourceEditorResult()
            {
                ExitCode = 0,
                ResultMessage = $"Current Set DB Now: {ResourceEditorConfiguration.GetSetDb()}"
            };
        }

        return new ResourceEditorResult()
        {
            ExitCode = 1,
            ResultMessage = "Database not found! No changes will be applied."
        };
    }
}