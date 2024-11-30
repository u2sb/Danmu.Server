using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DanMu.Models.BiliBili;

[Table(nameof(BiliBiliPagesCaching))]
[Index(nameof(BvId), IsUnique = true)]
public class BiliBiliPagesCaching
{
  [Key]
  [Column(nameof(BvId))]
  [MaxLength(12)]
  public required string BvId { get; init; }

  [NotMapped] public BiliBiliPages? Pages { get; set; }

  /// <summary>
  ///   数据
  /// </summary>
  [Column(nameof(Pages))]
  public byte[] PagesData { get; set; } = [];

  /// <summary>
  ///   缓存更新时间
  /// </summary>
  [Column(nameof(DateTime))]
  public DateTime DateTime { get; set; } = DateTime.UtcNow;
}