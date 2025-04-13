using ResourceEditorCli.Commands.Interfaces;
using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorCli.Commands;

public class CreateDbCommand : ResourceEditorCommand
{
    public ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as CreateDbOptions;
        
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
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = $"File {opts.Path} already exists."
            };
        }
        else
        {
            context.DbFilePath = opts.Path;
            context.Database.EnsureCreated();
            Console.WriteLine($"Created file {opts.Path}.");

            int ordinal = 0;
            
            context.Add(new DbInformationItem()
            {
                Ordinal = ordinal++,
                Name = "VersionMajor",
                Value = "1",
            });
            
            context.Add(new DbInformationItem()
            {
                Ordinal = ordinal++,
                Name = "VersionMinor",
                Value = "0",
            });
            
            context.Add(new DbInformationItem()
            {
                Ordinal = ordinal++,
                Name = "VersionBuild",
                Value = "0",
            });
            
            context.Add(new DbInformationItem()
            {
                Ordinal = ordinal++,
                Name = "DateCreated",
                Value = $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}",
            });
            
            context.SaveChanges();
            return new ResourceEditorResult()
            {
                ExitCode = 0,
            };
        }
    }
}