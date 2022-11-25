using DanMu.Models.Meting;
using DanMu.Models.Settings;
using DanMu.Utils.MetingHelp;
using Microsoft.AspNetCore.Mvc;
using static DanMu.Models.Meting.QueryType;

namespace DanMu.Controllers.Api.Meting;

[Route("api/music")]
[Route("api.php")]
[Route("api/meting")]
[ApiController]
public class MetingController : ControllerBase
{
    private readonly MetingHelp _meting;
    private readonly AppSettings _settings;

    public MetingController(MetingHelp meting, AppSettings settings)
    {
        _meting = meting;
        _settings = settings;
    }

    [HttpGet]
    public async Task<ActionResult<string?>> Get([FromQuery] QueryModel query)
    {
        if (string.IsNullOrEmpty(query.Id)) return BadRequest();

        var meting = query.Server.Equals(_settings.Meting.DefaultServerProvider)
            ? _meting
            : _meting.SetServer(query.Server);

        switch (query.Type)
        {
            case Lrc:
                return await meting.GetLrcAsync(query.Id);
            case Pic:
                var picUrl = meting.GetPicAsync(query.Id);
                return Redirect(await picUrl ?? "404");
            case QueryType.Url:
                var url = await meting.GetUrlAsync(query.Id);
                return Redirect(!string.IsNullOrWhiteSpace(url) ? url : "404");
            default:
                var request = HttpContext.Request;
                var baseUrl = string.IsNullOrEmpty(_settings.Meting.Url)
                    ? $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}"
                    : _settings.Meting.Url;
                return await meting.SearchAsync(query, baseUrl);
        }
    }
}