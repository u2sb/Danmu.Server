namespace DanMu.Models.BiliBili;

public class BQuery
{
    public int Aid { get; set; } = 0;
    public string BVid { get; set; } = string.Empty;
    public int P { get; set; } = 1;

    /// <summary>
    ///     类型 1视频 2漫画
    /// </summary>
    public int Type { get; set; } = 1;
}