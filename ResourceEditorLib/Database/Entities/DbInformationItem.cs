using System.ComponentModel.DataAnnotations;

namespace ResourceEditorLib.Database.Entities;

public class DbInformationItem
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public int Ordinal { get; set; }
}