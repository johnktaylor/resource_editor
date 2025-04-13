using Newtonsoft.Json.Linq;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;

namespace ResourceEditorCli.Commands;

public abstract class ResourceEditorCommand
{
    public abstract ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options);
    protected virtual string? GetSetDb()
    {
        var filePath = "appsettings.json";
        
        var json = File.ReadAllText(filePath);
        var config = JObject.Parse(json);
        
        config["Settings"] ??= new JObject();
        var setDb = config["Settings"]!["currentDb"];

        return setDb?.ToString();
    }
}