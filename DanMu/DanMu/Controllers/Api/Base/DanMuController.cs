using DanMu.Utils.Dao.Danmu;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.Base;

[FormatFilter]
[ApiController]
public class DanMuController : ControllerBase
{
    protected readonly DanmuTableDao Danmu;

    public DanMuController(DanmuTableDao danmu)
    {
        Danmu = danmu;
    }
}