using CommandLine;
using ResourceEditorCli.Options.Interfaces;

namespace ResourceEditorCli.Options;

[Verb("getresourcebyname", HelpText="Get resource from current set db by name")]
public class GetResourceByNameOptions : IResourceEditorOptions
{
    [Option('b', "binary", SetName="resourcetype", HelpText="Specify binary resource type")]
    public bool BinaryFlag { get; set; }
    
    [Option('t', "text", SetName="resourcetype", HelpText="Specify text resource type")]
    public bool TextFlag { get; set; }
    
    [Option('s', "namespace", Required=false, Default = "root", HelpText="Specify resource namespace")]
    public string? ResourceNameSpace { get; set; }
    
    [Option('n', "name", Required=true, HelpText="Specify resource name")]
    public string? ResourceName { get; set; }
    
    [Option('o', "outputfilename", Required=false, HelpText="Specify resource output filename")]
    public string? ResourceOutputFilename { get; set; }
}