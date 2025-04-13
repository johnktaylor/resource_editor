using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("deleteresourcebyid", HelpText="Get resource from current set db by name")]
public class DeleteResourceByIdOptions : IResourceEditorOptions
{
    [Option('i', "id", Required = true, HelpText="Specify Id of resource")]
    public string? Id { get; set; }
}