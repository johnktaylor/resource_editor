using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("getresourcebyid", HelpText="Get resource from current set db by name")]
public class GetResourceByIdOptions : IResourceEditorOptions
{
    [Option('i', "id", Required = true, HelpText="Specify Id of resource")]
    public string? Id { get; set; }
    
    [Option('f', "filename", Required = false, HelpText="Output filename")]
    public string? Filename { get; set; }
}