using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceEditorLib.Database.Entities;

public class DbInformationItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public int Ordinal { get; set; }
}