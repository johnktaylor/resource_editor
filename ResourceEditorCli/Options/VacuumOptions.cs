using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("shrink", HelpText = "Shrink the resource db")]
public class VacuumOptions : IResourceEditorOptions
{
    
}