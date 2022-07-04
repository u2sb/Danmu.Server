using System.ComponentModel.DataAnnotations;

namespace DanMu.Models.DataTables.DanMu;

public class VideoTable
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public string VideoId { get; set; } = null!;
    public List<string> PageUrl { get; set; } = new();
}