using System.ComponentModel.DataAnnotations;
using System.Net;
using DanMu.Models.DanMu.Generic.V1;

namespace DanMu.Models.DataTables.DanMu;

public class DanMuTable : GenericDanMu
{
    public DanMuTable()
    {
    }

    public DanMuTable(GenericDanMu danmu)
    {
        Id = Guid.TryParse(danmu.Id, out var id) ? id : Guid.NewGuid();
        Time = danmu.Time;
        Mode = danmu.Mode;
        FontSize = danmu.FontSize;
        Color = danmu.Color;
        Author = danmu.Author;
        AuthorId = danmu.AuthorId;
        Content = danmu.Content;
        CTime = danmu.CTime;
        Pool = danmu.Pool;
    }

    /// <summary>
    ///     弹幕ID
    /// </summary>
    [Key]
    public new Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     弹幕所在视频
    /// </summary>
    [Required]
    [MaxLength(72)]
    public string VideoId { get; set; } = null!;

    /// <summary>
    ///     弹幕发送者IP
    /// </summary>
    public IPAddress? Ip { get; set; }

    /// <summary>
    ///     关联到Video表
    /// </summary>
    public VideoTable Video { get; set; } = null!;
}