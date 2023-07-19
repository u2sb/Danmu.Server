using DanMu.Models.BiliBili;
using LiteDB;
using LiteDB.Async;

namespace DanMu.Utils.Caching;

public class BiliBiliCaching
{
  private readonly ILiteStorageAsync<int> _dmCaching;
  private readonly ILiteCollectionAsync<BiliBiliPagesCaching> _pagesCaching;

  public BiliBiliCaching(CachingContext context)
  {
    _pagesCaching = context.BiliBiliPagesCaching;
    _dmCaching = context.BiliDanMuCache;
  }

  /// <summary>
  ///   读取或设置Pages缓存
  /// </summary>
  /// <param name="key">键</param>
  /// <param name="factory"></param>
  /// <param name="expiration">过期时间</param>
  /// <returns></returns>
  public async Task<BiliBiliPages?> PagesGetOrSetAsync(string key, Func<Task<BiliBiliPages?>> factory,
    TimeSpan expiration)
  {
    var a = await _pagesCaching.FindOneAsync(x => x.BvId == key);

    if (a is { Pages.Data.Length: > 0 } && a.DateTime.Add(expiration) > DateTime.UtcNow)
      return a.Pages;

    var b = await factory.Invoke();

    if (b is { Code: 0 } and { Data.Length: > 0 })
      await _pagesCaching.UpsertAsync(new BiliBiliPagesCaching
      {
        _id = a?._id ?? ObjectId.NewObjectId(),
        BvId = key,
        Pages = b,
        DateTime = DateTime.UtcNow
      });
    return b;
  }

  /// <summary>
  ///   获取或设置弹幕缓存
  /// </summary>
  /// <param name="key">键</param>
  /// <param name="factory"></param>
  /// <param name="expiration">过期时间</param>
  /// <returns></returns>
  public async Task<Stream?> DmGetOrSetAsync(int key, Func<Task<Stream?>> factory,
    TimeSpan expiration)
  {
    var a = await _dmCaching.FindByIdAsync(key);
    if (a != null && a.UploadDate.Add(expiration) > DateTime.UtcNow)
    {
      var b = await _dmCaching.FindByIdAsync(key);

      if (b != null)
      {
        var ms = new MemoryStream();
        b.CopyTo(ms);
        return ms;
      }
    }

    var f = await factory.Invoke();
    if (f != null)
    {
      f.Position = 0;
      await _dmCaching.UploadAsync(key, key.ToString(), f);
    }

    return f;
  }
}