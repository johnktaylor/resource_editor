using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceEditorLib.Database.Entities;

public class ResourceHeader
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? ResourceNamespace { get; set; }
    
    public string? ResourceName { get; set; }
    
    public string? ResourceType { get; set; }
    
    public string? ExtendedAttributes { get; set; }
}