using ResourceEditorLib.Database;
using CommandLine;
using ResourceEditorLib.Database.Entities;
using Newtonsoft.Json.Linq;
using ResourceEditorCli.Commands;
using ResourceEditorCli.Options;

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
                AddResourceOptions, DeleteResourceByNameOptions>(args)
            .MapResult(
                (CreateDbOptions opts) => CreateDb(opts),
                (InfoDbOptions opts) => InfoDb(opts),
                (SetDbOptions opts) => SetDb(opts),
                (AddResourceOptions opts) => AddResource(opts),
                (DeleteResourceByNameOptions opts) => DeleteResourceByName(opts),
                errs => 1
            );
    }

    public void PrintHeader()
    {
        Console.WriteLine("Resource Editor");
        Console.WriteLine("---------------");
        //Console.WriteLine($"Current Set DB: {GetSetDb()}");
    }

    private void PrintResultIfNeeded(ResourceEditorResult result)
    {
        if (!string.IsNullOrEmpty(result.ResultMessage))
        {
            Console.WriteLine(result.ResultMessage);
        }

    }

    private int DeleteResourceByName(DeleteResourceByNameOptions opts)
    {
        var command = new DeleteResourceByNameCommand();
        var result = command.ExecuteCommand(Context, opts);
        PrintResultIfNeeded(result);
        return result.ExitCode;
    }
    
    private int AddResource(AddResourceOptions opts)
    {
        var command = new AddResourceCommand();
        var result = command.ExecuteCommand(Context, opts);
        PrintResultIfNeeded(result);
        return result.ExitCode;
    }

    private int SetDb(SetDbOptions opts)
    {
        var command = new SetDbCommand();
        var result = command.ExecuteCommand(Context, opts);
        PrintResultIfNeeded(result);
        return result.ExitCode;
    }

    private int InfoDb(InfoDbOptions opts)
    {
        var command = new InfoDbCommand();
        var result = command.ExecuteCommand(Context, opts);
        PrintResultIfNeeded(result);
        return result.ExitCode;
    }
    
    private int CreateDb(CreateDbOptions opts)
    {
        var command = new CreateDbCommand();
        var result = command.ExecuteCommand(Context, opts);
        PrintResultIfNeeded(result);
        return result.ExitCode;
    }
}