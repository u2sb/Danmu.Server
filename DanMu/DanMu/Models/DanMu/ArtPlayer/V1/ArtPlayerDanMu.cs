using DanMu.Models.DanMu.Generic.V1;

namespace DanMu.Models.DanMu.ArtPlayer.V1;

public class ArtPlayerDanMu
{
    /// <summary>
    ///     弹幕文本
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     弹幕时间
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    ///     弹幕颜色
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    ///     弹幕字号
    /// </summary>
    public int Size { get; set; } = 25;

    /// <summary>
    ///     是否有边框
    /// </summary>
    public bool Border { get; set; } = false;

    /// <summary>
    ///     弹幕模式 0-滚动 1-固定
    /// </summary>
    public int Mode { get; set; }

    public static explicit operator ArtPlayerDanMu(GenericDanMu data)
    {
        var t = data.Mode;
        switch (t)
        {
            case 4:
                t = 1;
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

        return new ArtPlayerDanMu
        {
            Time = data.Time / 1000f,
            Mode = t,
            Color = $"#{data.Color:X}",
            Size = data.FontSize,
            Text = data.Content
        };
    }

    public static explicit operator GenericDanMu(ArtPlayerDanMu data)
    {
        return new GenericDanMu
        {
            Time = (int)(data.Time * 1000),
            Mode = data.Mode == 0 ? 1 : 5,
            Color = Convert.ToUInt32(data.Color.Replace("#", "0x"), 16),
            FontSize = data.Size,
            Content = data.Text
        };
    }
}