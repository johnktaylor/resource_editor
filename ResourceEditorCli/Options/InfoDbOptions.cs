using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("infodb", HelpText = "Display DB information")]
public class InfoDbOptions : IResourceEditorOptions
{
    [Value(0, HelpText = "The path to the database file.")]
    public string? Path { get; set; }
}