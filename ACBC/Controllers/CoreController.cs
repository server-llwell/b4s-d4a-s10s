using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACBC.Controllers
{
    [Produces("application/json")]
    [Route(Global.ROUTE_PX + "/[controller]/[action]")]
    [EnableCors("AllowSameDomain")]
    public class CoreController : Controller
    {
        [HttpPost]
        public ActionResult Demos([FromBody]DemoApi demoApi)
        {
            if (demoApi == null)
                return Json(new ResultsJson(new Message(CodeMessage.PostNull, "PostNull"), null));
            return Json(Global.BUSS.BussResults(this, demoApi));
        }

    }
}