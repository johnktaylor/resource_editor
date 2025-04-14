using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ResourceEditorLib.Database.Entities;

[Index(nameof(ResourceNamespace), nameof(ResourceName), IsUnique = true)]
public class ResourceHeader
{
    [Key]
    public Guid Id { get; set; }
    public Guid ResourceId { get; set; }
    
    public string? FileName { get; set; }
    public string? ResourceNamespace { get; set; }
    public string? ResourceName { get; set; }
    public string? ResourceType { get; set; }
    public string? ExtendedAttributes { get; set; }
}