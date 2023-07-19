using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.Base;

[FormatFilter]
[ApiController]
public class BiliBiliDanMuBaseController : ControllerBase
{
  private protected readonly BiliBiliHelp Bilibili;

  public BiliBiliDanMuBaseController(BiliBiliHelp bilibili)
  {
    Bilibili = bilibili;
  }
}