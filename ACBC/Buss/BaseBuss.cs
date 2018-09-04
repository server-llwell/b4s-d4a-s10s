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
        /// 检查token判断执行权限
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private Message CheckToken(ApiType apiType, string userId, string token, string route)
        {
            Message msg = null;
#if !DEBUG
            if (userId != null)
            {
                using (var client = ConnectionMultiplexer.Connect(Global.REDIS))
                {
                    try
                    {
                        var db = client.GetDatabase(0);
                        var tokenRedis = db.StringGet(userId);
                        string tokenRedisStr = tokenRedis.ToString().Substring(1, tokenRedis.ToString().Length - 2);
                        if (token != tokenRedisStr)
                        {
                            Console.WriteLine(tokenRedis);
                            msg = new Message(CodeMessage.InvalidToken, "InvalidToken");
                        }
                        else
                        {
                            UserDao userDao = new UserDao();
                            if (!userDao.isAuth("/"+route, userId))
                            {
                                Console.WriteLine(tokenRedis);
                                msg = new Message(CodeMessage.InterfaceRole, "InterfaceRole");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        msg = new Message(CodeMessage.InvalidToken, "InvalidToken");
                    }
                }
            }
            else
            {
                Console.WriteLine(userId);
                msg = new Message(CodeMessage.InvalidToken, "InvalidToken");
            }
#endif
            return msg;
        }

        /// <summary>
        /// 执行业务方法
        /// </summary>
        /// <param name="apiType">API方法组</param>
        /// <param name="token">token</param>
        /// <param name="method">方法名</param>
        /// <param name="param">参数JSON对象</param>
        /// <returns></returns>
        public object BussResults(Controller controller, ApiType apiType, object param)
        {
            var route = "";
            var method = "";
            var routeController = controller.RouteData.Values["controller"].ToString();
            var routeAction = controller.RouteData.Values["action"].ToString();
            route = Global.ROUTE_PX + "/" + routeController + "/" + routeAction;
            method = routeAction.Replace("/", "");
            var s = controller.RouteData;
            var userId = "";
            var token = "";
            if (controller.Request.Headers.ContainsKey("userid") && controller.Request.Headers.ContainsKey("token"))
            {
                userId = controller.Request.Headers["userid"].ToString();
                token = controller.Request.Headers["token"].ToString();
            }
            
            var msg = CheckToken(apiType, userId, token, route);
            if (msg != null)
            {
                controller.Response.Headers.Add("code", ((int)msg.code).ToString());
                controller.Response.Headers.Add("msg", msg.msg);
                return "";
            }
            var obj = bussList[apiType];
            MethodInfo methodInfo = obj.GetType().GetMethod("Do_" + method);
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
                    data = methodInfo.Invoke(obj, new object[] { param, userId });
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
    }
}
