using System.ComponentModel.DataAnnotations;

namespace ResourceEditorLib.Database.Entities;

public class TextResource
{
    [Key]
    public Guid Id { get; set; }
    
    public string? Value { get; set; }
}