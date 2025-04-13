using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("setdb", HelpText = "Set Active DB")]
public class SetDbOptions : IResourceEditorOptions
{
    [Value(0, Required = true, HelpText = "The path to the database file.")]
    public string? Path { get; set; }
}