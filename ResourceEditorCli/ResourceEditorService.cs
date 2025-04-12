using ResourceEditorLib.Database;
using CommandLine;
using ResourceEditorLib.Database.Entities;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ResourceEditorCli;

public class ResourceEditorService : IResourceEditorService
{
    public ResourceDbContext Context { get; set; }

    public ResourceEditorService(IResourceDbContext dbContext)
    {
        Context = (ResourceDbContext)dbContext;
    }

    public int Run(string[] args)
    {
        PrintHeader();

        return Parser.Default.ParseArguments<
                CreateDbOptions, InfoDbOptions, SetDbOptions,
                AddResourceOptions, DeleteResourceOptions>(args)
            .MapResult(
                (CreateDbOptions opts) => CreateDb(opts),
                (InfoDbOptions opts) => InfoDb(opts),
                (SetDbOptions opts) => SetDb(opts),
                (AddResourceOptions opts) => 0,
                (DeleteResourceOptions opts) => 0,
                errs => 1
            );
    }

    public void PrintHeader()
    {
        Console.WriteLine("Resource Editor");
        Console.WriteLine("---------------");
        Console.WriteLine($"Current Set DB: {GetSetDb()}");
    }

    public string GetSetDb()
    {
        var filePath = "appsettings.json";
        
        var json = File.ReadAllText(filePath);
        var config = JObject.Parse(json);
        
        config["Settings"] ??= new JObject();
        var setDb = config["Settings"]!["currentDb"];

        return setDb == null ? "Not Set" : setDb.ToString();
    }

    private int SetDb(SetDbOptions opts)
    {
        var filePath = "appsettings.json";

        if (File.Exists(opts.Path))
        {
            // Read the existing configuration file into a JObject
            var json = File.ReadAllText(filePath);
            var config = JObject.Parse(json);

            // Assuming your settings are stored under a "Settings" section, update or add a value
            var newValue = opts.Path;

            config["Settings"] ??= new JObject();
            config["Settings"]!["currentDb"] = newValue;

            // Write the updated JSON back to the file
            File.WriteAllText(filePath, config.ToString(Newtonsoft.Json.Formatting.Indented));

            Console.WriteLine($"Current Set DB Now: {GetSetDb()}");
        }
        else
        {
            Console.WriteLine("Database not found! No changes will be applied.");
            return 1;
        }

        return 0;
    }

    private int InfoDb(InfoDbOptions opts)
    {
        if (File.Exists(opts.Path))
        {
            Context.DbFilePath = opts.Path;
            var dbInfo = Context.Set<DbInformationItem>().OrderBy(x=>x.Ordinal).ToList();
            foreach (var info in dbInfo)
            {
                Console.WriteLine($"{info.Name} : {info.Value}");    
            }

            return 0;
        }
        else
        {
            Console.WriteLine("File not found");
            return 1;
        }
    }
    
    private int CreateDb(CreateDbOptions opts)
    {
        if (File.Exists(opts.Path))
        {
            Console.WriteLine($"File {opts.Path} already exists.");
            return 1;
        }
        else
        {
            Context.DbFilePath = opts.Path;
            Context.Database.EnsureCreated();
            Console.WriteLine($"Created file {opts.Path}.");

            Context.Add(new DbInformationItem()
            {
                Ordinal = 0,
                Name = "VersionMajor",
                Value = "1",
            });
            
            Context.Add(new DbInformationItem()
            {
                Ordinal = 1,
                Name = "VersionMinor",
                Value = "0",
            });
            
            Context.Add(new DbInformationItem()
            {
                Ordinal = 2,
                Name = "VersionBuild",
                Value = "0",
            });
            
            Context.SaveChanges();
            return 0;
        }
    }
}

[Verb("createdb", HelpText = "Creates a new DB")]
public class CreateDbOptions
{
    [Value(0, Required = true, HelpText = "The path to the database file.")]
    public string? Path { get; set; }
}

[Verb("infodb", HelpText = "Display DB information")]
public class InfoDbOptions
{
    [Value(0, Required = true, HelpText = "The path to the database file.")]
    public string? Path { get; set; }
}

[Verb("setdb", HelpText = "Set Active DB")]
public class SetDbOptions
{
    [Value(0, Required = true, HelpText = "The path to the database file.")]
    public string? Path { get; set; }
}

[Verb("addresource", HelpText="Add resource to current set db")]
public class AddResourceOptions
{
    [Option('b', "binary", Required=true, SetName="resourcetype", HelpText="Specify binary resource type")]
    public bool BinaryFlag { get; set; }
    
    [Option('t', "text", Required=true, SetName="resourcetype", HelpText="Specify text resource type")]
    public bool TextFlag { get; set; }
    
    [Option('s', "namespace", Required=false, Default = "root", SetName="resourcenamespace", HelpText="Specify resource namespace")]
    public bool ResourceNameSpace { get; set; }
    
    [Option('n', "name", Required=true, SetName="resourcename", HelpText="Specify resource name")]
    public bool ResourceName { get; set; }
    
    [Option('r', "resourcefilename", Required=true, SetName="resourcefilename", HelpText="Specify resource filename")]
    public bool ResourceFileName { get; set; }
    
    [Option('e', "extendedattfilename", Required=true, SetName="resourceextfilename", HelpText="Specify extended attributes filename")]
    public bool ExtendedAttributesFileName { get; set; }
}

[Verb("deleteresource", HelpText="Delete resource from current set db")]
public class DeleteResourceOptions
{
    [Option('b', "binary", Required=true, SetName="resourcetype", HelpText="Specify binary resource type")]
    public bool BinaryFlag { get; set; }
    
    [Option('t', "text", Required=true, SetName="resourcetype", HelpText="Specify text resource type")]
    public bool TextFlag { get; set; }
    
    [Option('s', "namespace", Required=false, Default = "root", SetName="resourcenamespace", HelpText="Specify resource namespace")]
    public bool ResourceNameSpace { get; set; }
    
    [Option('n', "name", Required=true, SetName="resourcename", HelpText="Specify resource name")]
    public bool ResourceName { get; set; }
    
    [Option('i', "id", Required=false, SetName="resourceid", HelpText="Specify id")]
    public int ResourceId { get; set; }
}