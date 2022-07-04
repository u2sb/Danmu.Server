using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DanMu.Models.DanMu.ArtPlayer.V1;

public class ArtPlayerDanMuIn : ArtPlayerDanMu
{
    /// <summary>
    ///     视频的id
    /// </summary>
    [MaxLength(36)]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     评论者ip
    /// </summary>
    public IPAddress? Ip { get; set; } = IPAddress.None;

    /// <summary>
    ///     来源
    /// </summary>
    public string? Referer { get; set; }
}