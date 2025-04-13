using Newtonsoft.Json.Linq;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Commands;

public abstract class ResourceEditorCommand
{
    protected string? GetSetDb()
    {
        var filePath = "appsettings.json";
        
        var json = File.ReadAllText(filePath);
        var config = JObject.Parse(json);
        
        config["Settings"] ??= new JObject();
        var setDb = config["Settings"]!["currentDb"];

        return setDb?.ToString();
    }
}