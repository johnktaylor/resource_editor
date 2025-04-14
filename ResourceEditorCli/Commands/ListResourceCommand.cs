using System.Text;
using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorCli.Commands;

public class ListResourceCommand : ResourceEditorCommand
{
    public override ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as ListResourceOptions;
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }

        if (!opts.BinaryFlag && !opts.TextFlag && !opts.AllFlag)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "You need to specify either Binary or Text or All"
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
        
        var resourceHeaders = new List<ResourceHeader>();
        if (opts.TextFlag)
        {
            resourceHeaders = context.Set<ResourceHeader>().Where(r=>r.ResourceType == "Text").ToList();
        }
        else if (opts.BinaryFlag)
        {
            resourceHeaders = context.Set<ResourceHeader>().Where(r => r.ResourceType == "Binary").ToList();
        }
        else if (opts.AllFlag)
        {
            resourceHeaders = context.Set<ResourceHeader>().ToList();
        }

        if (opts.Namespace != null)
        {
            resourceHeaders = resourceHeaders.Where(r => r.ResourceNamespace == opts.Namespace).ToList();
        }
        
        if (opts.Name != null)
        {
            resourceHeaders = resourceHeaders.Where(r => r.ResourceName == opts.Name).ToList();
        }

        StringBuilder sb = new StringBuilder();
        foreach (var resourceHeader in resourceHeaders)
        {
            sb.AppendLine($"{resourceHeader.Id}: {resourceHeader.ResourceType}: {resourceHeader.ResourceNamespace}: {resourceHeader.ResourceName}: {resourceHeader.FileName}");
        }

        return new ResourceEditorResult()
        {
            ExitCode = 0,
            ResultMessage = sb.ToString()
        };
    }
}