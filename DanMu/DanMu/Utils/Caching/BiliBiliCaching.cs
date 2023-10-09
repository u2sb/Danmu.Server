using DanMu.Models.BiliBili;
using LiteDB;
using LiteDB.Async;

namespace DanMu.Utils.Caching;

public class BiliBiliCaching(CachingContext context)
{
  private readonly ILiteStorageAsync<int> _dmCaching = context.BiliDanMuCache;
  private readonly ILiteCollectionAsync<BiliBiliPagesCaching> _pagesCaching = context.BiliBiliPagesCaching;


  /// <summary>
  ///   获取或设置页面缓存
  /// </summary>
  /// <param name="key"></param>
  /// <param name="factory"></param>
  /// <param name="expiration"></param>
  /// <returns></returns>
  public async ValueTask<BiliBiliPages?> PagesGetOrSetAsync(string key, Func<Task<BiliBiliPages?>> factory,
    TimeSpan expiration)
  {
    var a = await _pagesCaching.FindOneAsync(x => x.BvId == key).ConfigureAwait(false);

    if (a is { Pages.Data.Length: > 0 } && a.DateTime.Add(expiration) > DateTime.UtcNow)
      return a.Pages;

    var b = await factory.Invoke().ConfigureAwait(false);

    if (b is { Code: 0 } and { Data.Length: > 0 })
      await _pagesCaching.UpsertAsync(new BiliBiliPagesCaching
      {
        _id = a?._id ?? ObjectId.NewObjectId(),
        BvId = key,
        Pages = b,
        DateTime = DateTime.UtcNow
      }).ConfigureAwait(false);
    return b;
  }

  /// <summary>
  ///   获取或设置弹幕缓存
  /// </summary>
  /// <param name="key"></param>
  /// <param name="factory"></param>
  /// <param name="expiration"></param>
  /// <returns></returns>
  public async ValueTask<Stream?> DmGetOrSetAsync(int key, Func<Task<Stream?>> factory,
    TimeSpan expiration)
  {
    var a = await _dmCaching.FindByIdAsync(key).ConfigureAwait(false);
    if (a != null && a.UploadDate.Add(expiration) > DateTime.UtcNow)
    {
      var b = await _dmCaching.FindByIdAsync(key).ConfigureAwait(false);

      if (b != null)
      {
        var ms = new MemoryStream();
        b.CopyTo(ms);
        return ms;
      }
    }

    var f = await factory.Invoke().ConfigureAwait(false);
    if (f != null)
    {
      f.Position = 0;
      await _dmCaching.UploadAsync(key, key.ToString(), f).ConfigureAwait(false);
    }

    return f;
  }
}