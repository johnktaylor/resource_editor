using System.ComponentModel.DataAnnotations;

namespace ResourceEditorLib.Database.Entities;

public class BlobResource
{
    [Key]
    public Guid Id { get; set; }
    
    public byte[]? Value { get; set; }
}