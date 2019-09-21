using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Danmaku.Model;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Dplayer.DanmakuList
{
    [Route("dplayer/danmakulist")]
    [Authorize]
    public class DanmakuListController : Controller
    {
        private readonly IDanmakuDao _dao;

        public DanmakuListController(IDanmakuDao dao)
        {
            _dao = dao;
        }


        public ActionResult Index()
        {
            return View(_dao.DanmakuBaseQuery());
        }

    }
}