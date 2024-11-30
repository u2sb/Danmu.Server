using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DanMu.Models.BiliBili;

[Table(nameof(BiliBiliDmCaching))]
[Index(nameof(Cid), IsUnique = true)]
public class BiliBiliDmCaching
{
  /// <summary>
  ///   Cid
  /// </summary>
  [Key]
  [Column(nameof(Cid))]
  public required int Cid { get; set; }

  /// <summary>
  ///   数据
  /// </summary>
  [Column(nameof(Data))]
  public byte[] Data { get; set; } = [];

  /// <summary>
  ///   缓存更新时间
  /// </summary>
  [Column(nameof(DateTime))]
  public DateTime DateTime { get; set; } = DateTime.UtcNow;
}