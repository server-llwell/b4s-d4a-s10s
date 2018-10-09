using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    public class OpenBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.OpenApi;
        }

        public object Do_Login(BaseApi baseApi)
        {
            LoginParam loginParam = JsonConvert.DeserializeObject<LoginParam>(baseApi.param.ToString());
            if (loginParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            var jsonResult = SnsApi.JsCode2Json(Global.APPID, Global.APPSECRET, loginParam.code);
            if (jsonResult.errcode == Senparc.Weixin.ReturnCode.请求成功)
            {
                AccessTokenContainer.Register(Global.APPID, Global.APPSECRET);
                var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key);
                
                SessionUser sessionUser = new SessionUser();
                sessionUser.userType = "";
                sessionUser.openid = sessionBag.OpenId;
                sessionBag.Name = JsonConvert.SerializeObject(sessionUser);
                SessionContainer.Update(sessionBag.Key, sessionBag);
                return new { token = sessionBag.Key, isReg = true };
            }
            else
            {
                throw new ApiException(CodeMessage.SenparcCode, jsonResult.errmsg);
            }
        }
    }
}
