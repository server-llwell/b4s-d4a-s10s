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
    [Consumes("multipart/form-data")]
    [Route(Global.ROUTE_PX + "/[controller]/[action]")]
    [EnableCors("AllowSameDomain")]
    public class UploadController : Controller
    {
        [HttpPost]
        [ActionName("Temp")]
        public ActionResult Temp(IFormCollection param)
        {
            return Json(Global.BUSS.BussResults(this, new UploadApi { param = param }));
        }

    }
}