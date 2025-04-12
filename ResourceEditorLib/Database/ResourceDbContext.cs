using Microsoft.EntityFrameworkCore;
using ResourceEditorLib.Database.Entities;

namespace ResourceEditorLib.Database;

public class ResourceDbContext : DbContext, IResourceDbContext
{
    public string? DbFilePath { get; set; }
    
    DbSet<BlobResource> BlobResources { get; set; }
    DbSet<TextResource> TextResources { get; set; }
    DbSet<ResourceHeader> ResourceHeaders { get; set; }
    DbSet<DbInformationItem> DbInformationItems { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (DbFilePath == null)
        {
            throw new NullReferenceException("DbFilePath cannot be null");
        }
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={DbFilePath}");
        }
    }
}