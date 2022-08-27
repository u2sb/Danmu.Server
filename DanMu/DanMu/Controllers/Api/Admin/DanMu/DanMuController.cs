using DanMu.Models.DataTables.DanMu;
using DanMu.Models.WebResults;
using DanMu.Utils.Dao.Danmu;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static DanMu.Models.Settings.ConstantTable;

namespace DanMu.Controllers.Api.Admin.DanMu;

[Authorize(Policy = AdminRolePolicy)]
[Route("/api/admin/danmu/")]
public class DanMuController : Base.DanMuController
{
    public DanMuController(DanmuTableDao danmu) : base(danmu)
    {
    }

    [HttpGet("list")]
    public async Task<WebResult<DanMuTable[]>> ListDanMuAsync()
    {
        var danmuList = await Danmu.GetAllDanMuAsync();

        return new WebResult<DanMuTable[]>(danmuList);
    }

    [HttpGet("delete/{id}")]
    public async Task<WebResult> DeleteDanMuAsync(string id)
    {
        if (!string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out var guid))
            if (await Danmu.DeleteDanMuAsync(guid))
                return new WebResult(0);

        return new WebResult(1);
    }

    [HttpPost("editor")]
    public async Task<WebResult> EditorDanMuAsync(DanMuTable danmu)
    {
        if (await Danmu.EditorDanMuAsync(danmu))
        {
            return new WebResult(0);
        }

        return new WebResult(1);
    }
}