using Bilibili.Community.Service.Dm.V1;

namespace DanMu.Models.DanMu.Generic.V1;

public class GenericDanMu
{
    /// <summary>
    ///     弹幕Id
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     弹幕视频中出现时间 毫秒
    /// </summary>
    public int Time { get; set; }

    /// <summary>
    ///     弹幕类型
    ///     1 2 3：普通弹幕
    ///     4：底部弹幕
    ///     5：顶部弹幕
    ///     6：逆向弹幕
    ///     7：高级弹幕
    ///     8：代码弹幕
    ///     9：BAS弹幕（仅限于特殊弹幕专包）
    /// </summary>
    public int Mode { get; set; }

    /// <summary>
    ///     弹幕字体大小
    ///     18：小
    ///     25：标准
    ///     36：大
    /// </summary>
    public int FontSize { get; set; }

    /// <summary>
    ///     弹幕颜色
    ///     十进制 RGB888
    /// </summary>
    public uint Color { get; set; }

    /// <summary>
    ///     弹幕作者 名称
    /// </summary>
    public string Author { get; set; } = "Anonymous";

    /// <summary>
    ///     弹幕作者ID
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    ///     弹幕内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    ///     弹幕发送时间 时间戳 毫秒
    /// </summary>
    public long CTime { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

    /// <summary>
    ///     弹幕池
    ///     0：普通池
    ///     1：字幕池
    ///     2：特殊池（代码/BAS弹幕）
    /// </summary>
    public int Pool { get; set; }


    public static explicit operator GenericDanMu(DanmakuElem data)
    {
        return new GenericDanMu
        {
            Id = data.IdStr,
            Time = data.Progress,
            Mode = data.Mode,
            FontSize = data.Fontsize,
            Color = data.Color,
            Author = data.MidHash,
            CTime = data.Ctime,
            Pool = data.Pool,
            Content = data.Content
        };
    }

    public static explicit operator DanmakuElem(GenericDanMu data)
    {
        return new DanmakuElem
        {
            IdStr = data.Id,
            Progress = data.Time,
            Mode = data.Mode,
            Fontsize = data.FontSize,
            Color = data.Color,
            MidHash = data.Author,
            Ctime = data.CTime,
            Pool = data.Pool,
            Content = data.Content
        };
    }
}