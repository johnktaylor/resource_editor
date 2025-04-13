using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("createdb", HelpText = "Creates a new DB")]
public class CreateDbOptions : IResourceEditorOptions
{
    [Value(0, Required = true, HelpText = "The path to the database file.")]
    public string? Path { get; set; }
}