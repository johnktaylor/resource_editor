using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorCli.Commands;

public class DeleteResourceByNameCommand : ResourceEditorCommand
{
    public override ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as DeleteResourceByNameOptions;
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }

        if (string.IsNullOrEmpty(opts.ResourceName) || string.IsNullOrEmpty(opts.ResourceNameSpace))
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "ResourceName and ResourceNameSpace are required"
            };
        }

        string type = "";
        if (opts.BinaryFlag)
        {
            type = "Binary";
        }
        else if (opts.TextFlag)
        {
            type = "Text";
        }

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
        
        var resourceheaders = context.Set<ResourceHeader>()
            .Where(r => 
                r.ResourceName == opts.ResourceName && 
                r.ResourceNamespace == opts.ResourceNameSpace &&
                r.ResourceType == type).ToList();

        if (!resourceheaders.Any())
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = $"No resource found with name {opts.ResourceName} and namespace {opts.ResourceNameSpace}"
            };
        }

        foreach (var resourceheader in resourceheaders)
        {
            Guid resourceid = resourceheader.ResourceId;
            switch (resourceheader.ResourceType)
            {
                case "Binary":
                {
                    var blobresources = context.Set<BlobResource>().Where(x=>x.Id == resourceid).ToList();
                    foreach (var blobresource in blobresources)
                    {
                        context.Set<BlobResource>().Remove(blobresource);
                    }

                    break;
                }
                case "Text":
                {
                    var textresources = context.Set<TextResource>().Where(x=>x.Id == resourceid).ToList();
                    foreach (var textresource in textresources)
                    {
                        context.Set<TextResource>().Remove(textresource);
                    }

                    break;
                }
            }
        }

        foreach (var resourceheader in resourceheaders)
        {
            context.Set<ResourceHeader>().Remove(resourceheader);
        }
        context.SaveChanges();

        return new ResourceEditorResult()
        {
            ExitCode = 0,
            ResultMessage = "Resources deleted"
        };
    }
}