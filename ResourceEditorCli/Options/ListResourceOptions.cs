using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("listresources", HelpText = "List Resources in Db")]
public class ListResourceOptions : IResourceEditorOptions
{
    [Option('b', "binary", SetName="resourcetype", HelpText="List binary resources")]
    public bool BinaryFlag { get; set; }
    
    [Option('t', "text", SetName="text", HelpText="List text resources")]
    public bool TextFlag { get; set; }
    
    [Option('a', "all", SetName="all", HelpText="List all resources")]
    public bool AllFlag { get; set; }
}