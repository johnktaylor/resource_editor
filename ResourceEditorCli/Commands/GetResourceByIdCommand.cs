using System.ComponentModel.Design;
using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorCli.Commands;

public class GetResourceByIdCommand : ResourceEditorCommand
{
    public override ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as GetResourceByIdOptions;
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }
        
        var setDb = ResourceEditorConfiguration.GetSetDb().ResultMessage;
        if (setDb == null)
        {
            var result = new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "You must set a db first."
            };
            return result;
        }

        context.DbFilePath = setDb;

        if (!Guid.TryParse(opts.Id, out Guid id))
        {
            var result = new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Id is not a Guid."
            };
            return result;
        }
        
        var resourceHeader = context.Set<ResourceHeader>()
            .FirstOrDefault(r => r.Id == id);

        if (resourceHeader == null)
        {
            return new ResourceEditorResult() { ExitCode = 1, ResultMessage = "Resource not found." };    
        }
        
        var filename = "";
        
        if (!string.IsNullOrEmpty(opts.Filename))
        {
            filename = opts.Filename;
        }
        else
        {
            filename = resourceHeader!.FileName;
        }
        
        var type = resourceHeader!.ResourceType;
        var length = 0;
        switch (type)
        {
            case "Text":
            {
                var value = context.Set<TextResource>().FirstOrDefault(x => x.Id == resourceHeader.ResourceId)?.Value;
                if (value != null)
                {
                    File.WriteAllText(filename!, value);
                    length = value.Length;
                }
                else
                {
                    return new ResourceEditorResult()
                    {
                        ExitCode = 1,
                        ResultMessage = "Nothing stored in database."
                    };
                }

                break;
            }
            case "Binary":
            {
                var value = context.Set<BlobResource>().FirstOrDefault(x => x.Id == resourceHeader.ResourceId)?.Value;
                if (value != null)
                {
                    File.WriteAllBytes(filename!, value);
                    length = value.Length;
                }
                else
                {
                    return new ResourceEditorResult()
                    {
                        ExitCode = 1,
                        ResultMessage = "Nothing stored in database."
                    };
                }

                break;
            }
        }

        return new ResourceEditorResult()
        {
            ExitCode = 0,
            ResultMessage = $"Exported {type}: {filename} ({length})"
        };
    }
}