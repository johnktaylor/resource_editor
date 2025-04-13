using Newtonsoft.Json.Linq;
using ResourceEditorCli.Commands.Interfaces;
using ResourceEditorCli.Options;
using ResourceEditorCli.Options.Interfaces;
using ResourceEditorLib.Database;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorCli.Commands;

public class AddResourceCommand : ResourceEditorCommand
{
    public ResourceEditorResult ExecuteCommand(ResourceDbContext context, IResourceEditorOptions options)
    {
        var opts = options as AddResourceOptions;
        
        if (opts == null)
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Wrong options provided"
            };
        }
        
        if (opts.ResourceFileName == null)
        {
            var results = new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Invalid resource filename."
            };
            return results;
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

        Guid? resourceGuid = null;

        if (opts.BinaryFlag)
        {
            if (File.Exists(opts.ResourceFileName))
            {
                var binary = context.Add(new BlobResource()
                {
                    Value = File.ReadAllBytes(opts.ResourceFileName),
                });

                resourceGuid = binary.Entity.Id;
            }
            else
            {
                return new ResourceEditorResult()
                {
                    ExitCode = 1,
                    ResultMessage = "Resource file does not exist."
                };
            }
        }
        else if (opts.TextFlag)
        {
            if (File.Exists(opts.ResourceFileName))
            {
                var text = context.Add(new TextResource()
                {
                    Value = File.ReadAllText(opts.ResourceFileName),
                });

                resourceGuid = text.Entity.Id;
            }
            else
            {
                return new ResourceEditorResult()
                {
                    ExitCode = 1,
                    ResultMessage = "Resource file does not exist."
                };
            }
        }
        else
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "You must specify either binary or text."
            };
        }

        if (resourceGuid.HasValue)
        {
            string? extendedAttributes = null;
            if (opts.ExtendedAttributesFileName != null)
            {
                extendedAttributes = File.ReadAllText(opts.ExtendedAttributesFileName);
            }
            
            context.Add(new ResourceHeader()
            {
                ResourceId = resourceGuid.Value,
                ResourceType = opts.BinaryFlag ? "Binary" : "Text",
                FileName = Path.GetFileName(opts.ResourceFileName),
                ExtendedAttributes = extendedAttributes,
                ResourceNamespace = opts.ResourceNameSpace,
                ResourceName = opts.ResourceName,
            });
            context.SaveChanges();
            Console.WriteLine($"Resource Added {resourceGuid}");
        }
        else
        {
            return new ResourceEditorResult()
            {
                ExitCode = 1,
                ResultMessage = "Unable to add resource."
            };
        }
        
        return new ResourceEditorResult()
        {
            ExitCode = 0,
        };
    }
}