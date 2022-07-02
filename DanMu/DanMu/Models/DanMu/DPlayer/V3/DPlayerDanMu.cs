using DanMu.Models.DanMu.Generic.V1;

namespace DanMu.Models.DanMu.DPlayer.V3;

public class DPlayerDanMu
{
    /// <summary>
    ///     弹幕时间 秒
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    ///     弹幕类型
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    ///     弹幕颜色
    /// </summary>
    public uint Color { get; set; }

    /// <summary>
    ///     弹幕作者
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    ///     弹幕文字
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     从GenericDanMu隐式转换为DPlayerDanMu
    /// </summary>
    /// <param name="data"></param>
    public static explicit operator DPlayerDanMu(GenericDanMu data)
    {
        var t = data.Mode;
        switch (t)
        {
            case 4:
                t = 2;
                break;
            case 5:
                t = 1;
                break;
            case 7:
                t = 0;
                data.Content = data.Content.Split(",")[4];
                break;
            case 8:
                t = 0;
                data.Content = string.Empty;
                break;
            default:
                t = 0;
                break;
        }

        return new DPlayerDanMu
        {
            Time = data.Time / 1000f,
            Type = t,
            Color = data.Color,
            Author = data.Author,
            Text = data.Content
        };
    }

    public static explicit operator GenericDanMu(DPlayerDanMu data)
    {
        var t = data.Type;

        switch (t)
        {
            case 0:
                t = 1;
                break;
            case 1:
                t = 5;
                break;
            case 2:
                t = 4;
                break;
            default:
                t = 0;
                break;
        }

        return new GenericDanMu
        {
            Time = (int)(data.Time * 1000),
            Mode = t,
            Color = data.Color,
            Author = data.Author ?? string.Empty,
            Content = data.Text ?? string.Empty
        };
    }
}