using System.Text.Json.Serialization;
using Meting4Net.Core;
using Meting4Net.Core.Models.Standard;

namespace DanMu.Models.Meting;

public class MusicList
{
    public MusicList()
    {
    }

    public MusicList(Music_search_item aItem, ServerProvider server, string baseUrl)
    {
        Title = aItem.name;
        Author = string.Join(" / ", aItem.artist);
        Url = $"{baseUrl}?server={server:G}&type=url&id={aItem.url_id}";
        Pic = $"{baseUrl}?server={server:G}&type=pic&id={aItem.pic_id}";
        Lrc = $"{baseUrl}?server={server:G}&type=lrc&id={aItem.lyric_id}";
    }

    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("author")] public string Author { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("pic")] public string Pic { get; set; }
    [JsonPropertyName("lrc")] public string Lrc { get; set; }
}