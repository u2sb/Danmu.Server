using Meting4Net.Core;

namespace DanMu.Models.Meting;

public enum QueryType
{
    Song,
    Album,
    Search,
    Artist,
    PlayList,
    Lrc,
    Url,
    Pic
}

public class QueryModel
{
    public ServerProvider Server { get; set; }
    public QueryType Type { get; set; }
    public string Id { get; set; }
}