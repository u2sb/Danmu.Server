using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.Base;

[FormatFilter]
[ApiController]
public class BiliBiliDanMuBaseController(BiliBiliHelp bilibili) : ControllerBase
{
}