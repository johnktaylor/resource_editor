using Newtonsoft.Json.Linq;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;

namespace ResourceEditorCli.Commands;

public abstract class ResourceEditorCommand
{
    public abstract ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options);
}