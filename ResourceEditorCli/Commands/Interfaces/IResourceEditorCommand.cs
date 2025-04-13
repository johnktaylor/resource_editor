using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;

namespace ResourceEditorCli.Commands.Interfaces;

public interface IResourceEditorCommand
{
    public ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options);
}