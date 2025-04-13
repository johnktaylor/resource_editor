using Newtonsoft.Json.Linq;
using ResourceEditorCli.Commands.Interfaces;
using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;

namespace ResourceEditorCli.Commands;

public class SetDbCommand : ResourceEditorCommand
{
    public ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var filePath = "appsettings.json";
        var opts = options as SetDbOptions;
        
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }
        
        if (File.Exists(opts.Path))
        {
            // Read the existing configuration file into a JObject
            var json = File.ReadAllText(filePath);
            var config = JObject.Parse(json);

            // Assuming your settings are stored under a "Settings" section, update or add a value
            var newValue = opts.Path;

            config["Settings"] ??= new JObject();
            config["Settings"]!["currentDb"] = newValue;

            // Write the updated JSON back to the file
            File.WriteAllText(filePath, config.ToString(Newtonsoft.Json.Formatting.Indented));

            return new ResourceEditorResult()
            {
                ExitCode = 0,
                ResultMessage = $"Current Set DB Now: {GetSetDb()}"
            };
        }

        return new ResourceEditorResult()
        {
            ExitCode = 1,
            ResultMessage = "Database not found! No changes will be applied."
        };
    }
}