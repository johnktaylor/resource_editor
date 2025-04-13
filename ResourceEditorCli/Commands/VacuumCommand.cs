using Microsoft.EntityFrameworkCore;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;

namespace ResourceEditorCli.Commands;

public class VacuumCommand :ResourceEditorCommand
{
    public override ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var setDb = GetSetDb();
        if (setDb == null)
        {
            var results = new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "You must set a db first."
            };
            return results;
        }

        context.DbFilePath = setDb;
        context.Database.ExecuteSql($"VACUUM");

        return new ResourceEditorResult()
        {
            ExitCode = 0,
            ResultMessage = "Database has been resized."
        };
    }
}