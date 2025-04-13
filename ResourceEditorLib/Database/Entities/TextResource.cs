using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceEditorLib.Database.Entities;

public class TextResource
{
    [Key]
    public Guid Id { get; set; }
    
    public string? Value { get; set; }
}