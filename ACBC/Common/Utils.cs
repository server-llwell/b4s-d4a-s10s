using ACBC.Buss;
using Newtonsoft.Json;
using Senparc.Weixin.Cache.Redis;
using Senparc.Weixin.WxOpen.Containers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class Utils
    {
        /// <summary>
        /// 获取系统已登录用户OPENID
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetOpenID(string token)
        {
            SessionBag sessionBag = SessionContainer.GetSession(token);
            if (sessionBag == null)
            {
                return null;
            }
            return sessionBag.OpenId;
        }

        public static bool SetCache(string key, object value, int hours, int minutes, int seconds)
        {
            key = Global.NAMESPACE + "." + key;
            var db = RedisManager.Manager.GetDatabase(Global.REDIS_NO);
            var expiry = new TimeSpan(hours, minutes, seconds);
            string valueStr = JsonConvert.SerializeObject(value);
            return db.StringSet(key, valueStr, expiry);
        }

        public static bool SetCache(BussCache value, int hours, int minutes, int seconds)
        {
            string key = value.GetType().FullName + value.Unique;
            return SetCache(key, value, hours, minutes, seconds);
        }

        public static bool SetCache(BussCache value)
        {
            return SetCache(value, Global.REDIS_EXPIRY_H, Global.REDIS_EXPIRY_M, Global.REDIS_EXPIRY_S);
        }

        public static dynamic GetCache<T>(string key)
        {
            key = Global.NAMESPACE + "." + key;
            var db = RedisManager.Manager.GetDatabase(Global.REDIS_NO);
            if (db.StringGet(key).HasValue)
            {
                return JsonConvert.DeserializeObject<T>(db.StringGet(key));
            }
            return null;
        }

        public static dynamic GetCache<T>()
        {
            string key = typeof(T).FullName;
            return GetCache<T>(key);
        }

        public static dynamic GetCache<T>(BussParam bussParam)
        {
            string key = typeof(T).FullName + bussParam.GetUnique();
            return GetCache<T>(key);
        }

        public static void DeleteCache(string key)
        {
            var db = RedisManager.Manager.GetDatabase(Global.REDIS_NO);
            if (db.StringGet(key).HasValue)
            {
                db.KeyDelete(key);
            }
        }

        public static void DeleteCacheAll(string key)
        {
            key = Global.NAMESPACE + "." + key + "*";
            var db = RedisManager.Manager.GetDatabase(Global.REDIS_NO);
            var server = RedisManager.Manager.GetServer(Global.REDIS);
            var keys = server.Keys(database: db.Database, pattern: key);
            db.KeyDelete(keys.ToArray());
        }

        public static void DeleteCache<T>(bool delChild = false)
        {
            string key = typeof(T).FullName;
            if (delChild)
            {
                DeleteCacheAll(key);
            }
            else
            {
                DeleteCache(key);
            }
        }

        public static void ClearCache()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                  .SelectMany(a => a.GetTypes()
                  .Where(t => typeof(BussCache).Equals(t.BaseType)))
                  .ToArray();
            foreach (var v in types)
            {
                if (v.IsClass)
                {
                    DeleteCacheAll(v.FullName);
                }
            }
        }

        public static string PostHttp(string url, string body, string contentType)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000;

            byte[] btBodys = Encoding.UTF8.GetBytes(body);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }

        public static string GetHttp(string url)
        {

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }

        public static double GetExchange(string name)
        {
            ExchangeRes exchangeRes = GetCache<ExchangeRes>("EXCHANGE");
            if (exchangeRes == null)
            {
                string exchange = GetHttp(Global.EXCHANGE_URL);
                exchangeRes = JsonConvert.DeserializeObject<ExchangeRes>(exchange);
                SetCache("EXCHANGE", exchangeRes, 1, 0, 0);
            }

            double exRate = 0;
            if (exchangeRes.error_code == 0)
            {
                var list = exchangeRes.result.list;
                foreach (string[] item in list)
                {
                    foreach (string s in item)
                    {
                        if (s != name)
                            break;
                        exRate = Convert.ToDouble(item[3]);
                        break;
                    }
                    if (exRate > 0)
                        break;
                }
            }

            return exRate / 100;
        }
    }
}
