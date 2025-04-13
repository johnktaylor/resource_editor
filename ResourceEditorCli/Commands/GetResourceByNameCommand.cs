using ResourceEditorCli.Commands.Interfaces;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;

namespace ResourceEditorCli.Commands;

public class GetResourceByNameCommand : ResourceEditorCommand
{
    public static ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        throw new NotImplementedException();
    }
}