using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceEditorLib.Database.Entities;

public class BlobResource
{
    [Key]
    public Guid Id { get; set; }
    
    public byte[]? Value { get; set; }
}