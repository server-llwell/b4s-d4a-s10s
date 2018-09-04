using ACBC.Common;
using ACBC.Dao;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace ACBC.Buss
{
    /// <summary>
    /// 具体业务实现类接口
    /// </summary>
    public interface IBuss
    {
        ApiType GetApiType();
    }

    /// <summary>
    /// 基础业务实现类
    /// </summary>
    public class BaseBuss
    {
        /// <summary>
        /// 具体业务实现类字典列表
        /// </summary>
        private Dictionary<ApiType, IBuss> bussList = new Dictionary<ApiType, IBuss>();

        /// <summary>
        /// 构造函数
        /// 构造时反射加载所有具体业务类
        /// </summary>
        public BaseBuss()
        {
            this.BuildBuss();
        }

        /// <summary>
        /// 遍历所有IBUSS接口的实现类
        /// 全部实例化并插入具体业务实现类字典列表
        /// </summary>
        private void BuildBuss()
        {
            ///获取所有IBUSS的实现类
            var types = AppDomain.CurrentDomain.GetAssemblies()
                  .SelectMany(a => a.GetTypes()
                  .Where(t => t.GetInterfaces().Contains(typeof(IBuss))))
                  .ToArray();
            ///遍历实现类，实例化非接口与抽象类，并插入具体业务实现类字典列表
            foreach (var v in types)
            {
                if (v.IsClass)
                {
                    var buss = (Activator.CreateInstance(v) as IBuss);
                    AddBuss(buss.GetApiType(), buss);
                }
            }
        }

        /// <summary>
        /// 插入具体业务实现类字典列表
        /// 相同组名的业务实现类唯一
        /// </summary>
        /// <param name="apiType">API方法组</param>
        /// <param name="buss">具体业务实现类</param>
        private void AddBuss(ApiType apiType, IBuss buss)
        {
            if (bussList.ContainsKey(apiType))
            {
                bussList[apiType] = buss;
            }
            else
            {
                bussList.Add(apiType, buss);
            }
        }

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="baseApi">传入参数</param>
        /// <param name="route">API路径</param>
        /// <returns>验证结果，null为通过</returns>
        private Message CheckToken(BaseApi baseApi, string route)
        {
            Message msg = null;
            if (baseApi.code != null)
            {
                using (var client = ConnectionMultiplexer.Connect(Global.REDIS))
                {
                    try
                    {
                        var db = client.GetDatabase(Global.REDIS_NO);
                        var tokenRedis = db.StringGet(baseApi.code);
                        string tokenRedisStr = tokenRedis.ToString();
                        if (baseApi.token != tokenRedisStr)
                        {
                            Console.WriteLine(tokenRedis);
                            msg = new Message(CodeMessage.InvalidToken, "InvalidToken");
                        }
                        else
                        {
                            AuthDao authDao = new AuthDao();
                            if (!authDao.CheckAuth("/" + route, baseApi.code))
                            {
                                Console.WriteLine(tokenRedis);
                                msg = new Message(CodeMessage.InterfaceRole, "InterfaceRole");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        msg = new Message(CodeMessage.InvalidToken, "InvalidToken");
                    }
                }
            }
            else
            {
                msg = new Message(CodeMessage.InvalidToken, "InvalidToken");
            }
            return msg;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="baseApi">传入参数</param>
        /// <param name="route">API路径</param>
        /// <returns>验证结果，null为通过</returns>
        private Message CheckSign(BaseApi baseApi, string route)
        {
            Message msg = null;
            if (baseApi.code != null)
            {
                string appSecret = new AuthDao().GetAccess(baseApi.code, baseApi.appId);
                if (appSecret == null)
                {
                    msg = new Message(CodeMessage.AppIDError, "AppIDError");
                }
                else
                {
                    string placeHold = "__PLACEHOLD__";
                    string paramS = Regex.Replace(
                        baseApi.param.ToString(), 
                        "\"(.+?)\"", 
                        new MatchEvaluator(
                            (s) => 
                            {
                                return s.ToString().Replace(" ", placeHold);
                            }))
                            .Replace("\n", "")
                            .Replace("\r", "")
                            .Replace(" ", "")
                            .Replace(placeHold, " ");
                    string needMd5 = baseApi.appId + baseApi.nonceStr + appSecret + paramS;
                    string md5S = "";
                    using (var md5 = MD5.Create())
                    {
                        var result = md5.ComputeHash(Encoding.UTF8.GetBytes(needMd5));
                        var strResult = BitConverter.ToString(result);
                        md5S = strResult.Replace("-", "");
                    }
                    if(baseApi.sign != md5S)
                    {
                        msg = new Message(CodeMessage.SignError, "SignError");
                    }
                }
            }
            else
            {
                msg = new Message(CodeMessage.InvalidToken, "InvalidToken");
            }
            return msg;
        }

        /// <summary>
        /// Header传递关键参数处理方法
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="baseApi">传入参数</param>
        /// <returns></returns>
        public object HeaderBussResults(Controller controller, BaseApi baseApi)
        {
            var route = "";
            var action = "";
            var routeController = controller.RouteData.Values["controller"].ToString();
            var routeAction = controller.RouteData.Values["action"].ToString();
            route = Global.ROUTE_PX + "/" + routeController + "/" + routeAction;
            action = routeAction.Replace("/", "");

            if (controller.Request.Headers.ContainsKey("userid"))
            {
                baseApi.code = controller.Request.Headers["userid"].ToString();
            }
            if (controller.Request.Headers.ContainsKey("token"))
            {
                baseApi.token = controller.Request.Headers["token"].ToString();
            }
            if (controller.Request.Headers.ContainsKey("sign") && controller.Request.Headers.ContainsKey("nonceStr"))
            {
                baseApi.sign = controller.Request.Headers["sign"].ToString();
                baseApi.nonceStr = controller.Request.Headers["nonceStr"].ToString();
            }

            Message msg = null;
            switch (baseApi.GetCheckType())
            {
                case CheckType.Open:
                    break;
                case CheckType.Token:
                    msg = CheckToken(baseApi, route);
                    break;
                case CheckType.Sign:
                    msg = CheckSign(baseApi, route);
                    break;
                default:
                    break;
            }

            if (msg != null)
            {
                controller.Response.Headers.Add("code", ((int)msg.code).ToString());
                controller.Response.Headers.Add("msg", msg.msg);
                return "";
            }
            var obj = bussList[baseApi.GetApiType()];
            MethodInfo methodInfo = obj.GetType().GetMethod("Do_" + action);
            if (methodInfo == null)
            {
                controller.Response.Headers.Add("code", ((int)CodeMessage.InvalidMethod).ToString());
                controller.Response.Headers.Add("msg", "InvalidMethod");
                return "";
            }
            else
            {
                Message message = null;
                object data = null;
                try
                {
                    data = methodInfo.Invoke(obj, new object[] { baseApi });
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.GetType() == typeof(ApiException))
                    {
                        ApiException apiException = (ApiException)ex.InnerException;
                        message = new Message(apiException.code, apiException.msg);
                    }
                    else
                    {
                        message = new Message(CodeMessage.InnerError, "InnerError");
                    }
                }
                if (message != null)
                {
                    controller.Response.Headers.Add("code", ((int)message.code).ToString());
                    controller.Response.Headers.Add("msg", message.msg);
                }
                else
                {
                    controller.Response.Headers.Add("code", ((int)CodeMessage.OK).ToString());
                    controller.Response.Headers.Add("msg", "");
                }

                return data;
            }
        }

        /// <summary>
        /// Body传递关键参数处理方法
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="baseApi">传入参数</param>
        /// <returns></returns>
        public object BodyBussResults(Controller controller, BaseApi baseApi)
        {
            var route = "";
            var action = "";
            var routeController = controller.RouteData.Values["controller"].ToString();
            var routeAction = controller.RouteData.Values["action"].ToString();
            route = Global.ROUTE_PX + "/" + routeController + "/" + routeAction + "/" + baseApi.method;
            action = routeAction.Replace("/", "");
            Message msg = null;
            switch(baseApi.GetCheckType())
            {
                case CheckType.Open:
                    break;
                case CheckType.Token:
                    msg = CheckToken(baseApi, route);
                    break;
                case CheckType.Sign:
                    msg = CheckSign(baseApi, route);
                    break;
                default:
                    break;
            }
            
            if (msg != null)
            {
                return new ResultsJson(new Message(CodeMessage.InvalidToken, "InvalidToken"), null);
            }
            var obj = bussList[baseApi.GetApiType()];
            MethodInfo methodInfo = obj.GetType().GetMethod("Do_" + baseApi.method);
            if (methodInfo == null)
            {
                return new ResultsJson(new Message(CodeMessage.InvalidMethod, "InvalidMethod"), null);
            }
            else
            {
                Message message = null;
                object data = null;
                try
                {
                    data = methodInfo.Invoke(obj, new object[] { baseApi });
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.GetType() == typeof(ApiException))
                    {
                        ApiException apiException = (ApiException)ex.InnerException;
                        message = new Message(apiException.code, apiException.msg);
                    }
                    else
                    {
                        message = new Message(CodeMessage.InnerError, "InnerError");
                    }
                }

                return new ResultsJson(message, data);
            }
        }


    }
}
