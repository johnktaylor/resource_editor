using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("addresource", HelpText="Add resource to current set db")]
public class AddResourceOptions : IResourceEditorOptions
{
    [Option('b', "binary", SetName="resourcetype", HelpText="Specify binary resource type")]
    public bool BinaryFlag { get; set; }
    
    [Option('t', "text", SetName="resourcetype", HelpText="Specify text resource type")]
    public bool TextFlag { get; set; }
    
    [Option('s', "namespace", Required=false, Default = "root", HelpText="Specify resource namespace")]
    public string? ResourceNameSpace { get; set; }
    
    [Option('n', "name", Required=true, HelpText="Specify resource name")]
    public string? ResourceName { get; set; }
    
    [Option('r', "resourcefilename", Required=true, HelpText="Specify resource filename")]
    public string? ResourceFileName { get; set; }
    
    [Option('e', "extendedattfilename", Required=false, HelpText="Specify extended attributes filename")]
    public string? ExtendedAttributesFileName { get; set; }
}