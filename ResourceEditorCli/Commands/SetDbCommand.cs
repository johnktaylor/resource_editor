using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;

namespace ResourceEditorCli.Commands;

public class SetDbCommand : ResourceEditorCommand
{
    public override ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as SetDbOptions;
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }
        
        return ResourceEditorConfiguration.SetDb(opts.Path!);
    }
}