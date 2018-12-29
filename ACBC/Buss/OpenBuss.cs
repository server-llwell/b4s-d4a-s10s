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

                //    SessionUser sessionUser = new SessionUser();
                //    sessionUser.userType = "";
                //    //sessionUser.openid = sessionBag.OpenId;
                //    sessionBag.Name = JsonConvert.SerializeObject(sessionUser);
                //    SessionContainer.Update(sessionBag.Key, sessionBag);
                //    return new { token = sessionBag.Key, isReg = true };
                //}
                //else
                //{
                //    throw new ApiException(CodeMessage.SenparcCode, jsonResult.errmsg);
                //}

                OpenDao openDao = new OpenDao();
                SessionUser sessionUser = new SessionUser();

                User user = openDao.GetUser(Utils.GetOpenID(sessionBag.Key));
                if (user == null)
                {
                    sessionUser.userType = "UNKWON";
                    sessionBag.Name = JsonConvert.SerializeObject(sessionUser);
                    SessionContainer.Update(sessionBag.Key, sessionBag);
                    return new { token = sessionBag.Key, isReg = false };
                }
                else
                {
                    sessionUser.userType = "USER";
                    sessionUser.openid = sessionBag.OpenId;
                    sessionUser.userId = user.userId;
                    sessionBag.Name = JsonConvert.SerializeObject(sessionUser);
                    SessionContainer.Update(sessionBag.Key, sessionBag);
                    return new
                    {
                        token = sessionBag.Key,
                        isReg = true,
                        user.openid,
                        user.userId,
                        user.userImg,
                        user.userName,
                        user.userType
                    };
                }
            }
            else
            {
                throw new ApiException(CodeMessage.SenparcCode, jsonResult.errmsg);
            }
        }

        public object Do_UserReg(BaseApi baseApi)
        {
            UserRegParam userRegParam = JsonConvert.DeserializeObject<UserRegParam>(baseApi.param.ToString());
            if (userRegParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            SessionBag sessionBag = SessionContainer.GetSession(baseApi.token);
            if (sessionBag == null)
            {
                throw new ApiException(CodeMessage.InvalidToken, "InvalidToken");
            }

            OpenDao openDao = new OpenDao();
            string openID = Utils.GetOpenID(baseApi.token);
            User user = openDao.GetUser(openID);

            if (user != null)
            {
                throw new ApiException(CodeMessage.UserExist, "UserExist");
            }

            //if (!openDao.UserReg(userRegParam, openID))
            //{
            //    throw new ApiException(CodeMessage.UserRegError, "UserRegError");
            //}
            user = openDao.GetUser(openID);
            SessionUser sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionBag.Name);
            sessionUser.openid = sessionBag.OpenId;
            sessionUser.userId = user.userId;
            sessionUser.userType = "STORE";
            sessionBag.Name = JsonConvert.SerializeObject(sessionUser);
            SessionContainer.Update(sessionBag.Key, sessionBag);

            return "";
        }
    }
}
