using System.Text;
using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorCli.Commands;

public class InfoDbCommand : ResourceEditorCommand
{
    public override ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as InfoDbOptions;
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }
        
        var dbPath = opts.Path ?? ResourceEditorConfiguration.GetSetDb().ResultMessage;
        if (dbPath == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "You must provide a path to an existing database (or set db).",
            };
        }
        
        if (File.Exists(dbPath))
        {
            context.DbFilePath = dbPath;
            var dbInfo = context.Set<DbInformationItem>().OrderBy(x=>x.Ordinal).ToList();
            var sb = new StringBuilder();
            foreach (var info in dbInfo)
            {
                sb.AppendLine($"{info.Name} : {info.Value}");    
            }

            return new ResourceEditorResult()
            {
                ExitCode = 0,
                ResultMessage = sb.ToString(),
            };
        }
        else
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "File not found.",
            };
        }
    }
}