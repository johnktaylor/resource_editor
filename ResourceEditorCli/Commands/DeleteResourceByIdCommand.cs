using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorCli.Commands;

public class DeleteResourceByIdCommand : ResourceEditorCommand
{
    public override ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as DeleteResourceByIdOptions;
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }

        if (!Guid.TryParse(opts.Id, out Guid id))
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Id is not a Guid"
            };
        }
        
        var setDb = ResourceEditorConfiguration.GetSetDb().ResultMessage;
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
        
        var resourceheader = context.Set<ResourceHeader>()
            .FirstOrDefault(r => r.Id == id);

        if (resourceheader == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Resource not found."
            };
        }
        
        var type = resourceheader.ResourceType;

        switch (type)
        {
            case "Binary":
                var blobresources = context.Set<BlobResource>().Where(x=>x.Id == resourceheader.ResourceId).ToList();
                foreach (var blobresource in blobresources)
                {
                    context.Set<BlobResource>().Remove(blobresource);
                }

                break;
            case "Text":
                var textresources = context.Set<TextResource>().Where(x=>x.Id == resourceheader.ResourceId).ToList();
                foreach (var textresource in textresources)
                {
                    context.Set<TextResource>().Remove(textresource);
                }

                break;
        }
        
        context.Set<ResourceHeader>().Remove(resourceheader);
        context.SaveChanges();

        return new ResourceEditorResult()
        {
            ExitCode = 0,
            ResultMessage = "Resource deleted."
        };
    }
}