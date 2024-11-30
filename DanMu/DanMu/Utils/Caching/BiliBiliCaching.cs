using System.Buffers;
using System.Text.Json;
using DanMu.Models.BiliBili;
using Microsoft.EntityFrameworkCore;
using ProtoBuf;

namespace DanMu.Utils.Caching;

public class BiliBiliCaching(CachingContext context)
{
  private readonly DbSet<BiliBiliDmCaching> _dmCaching = context.BiliBiliDmCaching;
  private readonly DbSet<BiliBiliPagesCaching> _pagesCaching = context.BiliBiliPagesCaching;

  /// <summary>
  ///   获取或设置页面缓存
  /// </summary>
  /// <param name="key"></param>
  /// <param name="factory"></param>
  /// <param name="expiration"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  public async ValueTask<BiliBiliPages?> PagesGetOrSetAsync(string key,
    Func<CancellationToken, ValueTask<Stream>> factory,
    TimeSpan expiration, CancellationToken ct = default)
  {
    var a = await _pagesCaching.FirstOrDefaultAsync(x => x.BvId == key, ct)
      .ConfigureAwait(false);

    if (a != null && a.DateTime.Add(expiration) > DateTime.UtcNow)
    {
      await using var ms = new MemoryStream(a.PagesData);
      return await JsonSerializer.DeserializeAsync<BiliBiliPages>(ms, cancellationToken: ct);
    }

    var f = await factory.Invoke(ct).ConfigureAwait(false);
    if (f == Stream.Null) return null;

    var pages = await JsonSerializer.DeserializeAsync<BiliBiliPages>(f, cancellationToken: ct);

    await using var ms1 = new MemoryStream();
    await JsonSerializer.SerializeAsync(ms1, pages, cancellationToken: ct);

    if (a == null)
    {
      var bpc = new BiliBiliPagesCaching
      {
        BvId = key,
        PagesData = ms1.ToArray(),
        DateTime = DateTime.UtcNow
      };
      _pagesCaching.Add(bpc);
    }
    else
    {
      a.PagesData = ms1.ToArray();
      a.DateTime = DateTime.UtcNow;
    }

    await context.SaveChangesAsync(ct);

    return pages;
  }

  /// <summary>
  ///   获取或设置弹幕缓存
  /// </summary>
  /// <param name="key"></param>
  /// <param name="factory"></param>
  /// <param name="expiration"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  public async ValueTask<DmSegMobileReply?> DmGetOrSetAsync(int key,
    Func<CancellationToken, ValueTask<DmSegMobileReply?>> factory,
    TimeSpan expiration, CancellationToken ct = default)
  {
    var a = await _dmCaching.FirstOrDefaultAsync(x => x.Cid == key, ct)
      .ConfigureAwait(false);
    if (a != null && a.DateTime.Add(expiration) > DateTime.UtcNow)
      return Serializer.Deserialize<DmSegMobileReply>(a.Data.AsSpan());

    var f = await factory.Invoke(ct).ConfigureAwait(false);
    if (f is not { Elems.Count: > 0 }) return f;

    var buffer = new ArrayBufferWriter<byte>();
    Serializer.Serialize(buffer, f);
    var b = buffer.WrittenSpan;

    if (a == null)
    {
      var bdc = new BiliBiliDmCaching
      {
        Cid = key,
        Data = b.ToArray(),
        DateTime = DateTime.UtcNow
      };
      _dmCaching.Add(bdc);
    }
    else
    {
      a.Data = b.ToArray();
      a.DateTime = DateTime.UtcNow;
    }

    await context.SaveChangesAsync(ct);

    return f;
  }
}