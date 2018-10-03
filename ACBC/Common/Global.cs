using ACBC.Buss;
using ACBC.Dao;
using Com.ACBC.Framework.Database;
using System;
using StackExchange.Redis;
using Senparc.Weixin.Cache.Redis;
using Senparc.Weixin.Cache;
using Senparc.Weixin.WxOpen.Containers;

namespace ACBC.Common
{
    public class Global
    {
        public const string ROUTE_PX = "/api/gift";
        public const int REDIS_NO = 1;
        public const int REDIS_EXPIRY = 7200;

        public const string SMS_CODE_URL = "http://v.juhe.cn/sms/send?mobile={0}&tpl_id=68600&tpl_value=%23code%23%3D{1}&dtype=&key=7c21d791256af1ffdd85375c64846358";
        public const string EXCHANGE_URL = "http://op.juhe.cn/onebox/exchange/query?key=08940f90d07501ace3f535e32968cf94";

        /// <summary>
        /// 基础业务处理类对象
        /// </summary>
        public static BaseBuss BUSS = new BaseBuss();

        /// <summary>
        /// 初始化启动预加载
        /// </summary>
        public static void StartUp()
        {
            if (DatabaseOperationWeb.TYPE == null)
            {
                DatabaseOperationWeb.TYPE = new DBManager();
            }

            try
            {
                RedisManager.ConfigurationOption = REDIS;
                CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisContainerCacheStrategy.Instance );
            }
            catch
            {
                Console.WriteLine("Redis Error, Change Local");
            }
        }

        
        public static string REDIS
        {
            get
            {
#if DEBUG
                var redis = System.Environment.GetEnvironmentVariable("redis", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var redis = "redis-api";
#endif
                return redis;
            }
        }

        #region 小程序相关

        /// <summary>
        /// 小程序APPID
        /// </summary>
        public static string APPID
        {
            get
            {
#if DEBUG
                var appId = System.Environment.GetEnvironmentVariable("WxAppId", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var appId = System.Environment.GetEnvironmentVariable("WxAppId");
#endif
                return appId;
            }
        }

        /// <summary>
        /// 小程序APPSECRET
        /// </summary>
        public static string APPSECRET
        {
            get
            {
#if DEBUG
                var appSecret = System.Environment.GetEnvironmentVariable("WxAppSecret", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var appSecret = System.Environment.GetEnvironmentVariable("WxAppSecret");
#endif
                return appSecret;
            }
        }


        #endregion

        #region OSS相关

        /// <summary>
        /// AccessId
        /// </summary>
        public static string AccessId
        {
            get
            {
#if DEBUG
                var accessId = System.Environment.GetEnvironmentVariable("ossAccessId", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var accessId = System.Environment.GetEnvironmentVariable("ossAccessId");
#endif
                return accessId;
            }
        }
        /// <summary>
        /// AccessKey
        /// </summary>
        public static string AccessKey
        {
            get
            {
#if DEBUG
                var accessKey = System.Environment.GetEnvironmentVariable("ossAccessKey", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var accessKey = System.Environment.GetEnvironmentVariable("ossAccessKey");
#endif
                return accessKey;
            }
        }
        /// <summary>
        /// OssHttp
        /// </summary>
        public static string OssHttp
        {
            get
            {
#if DEBUG
                var ossHttp = System.Environment.GetEnvironmentVariable("ossHttp", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var ossHttp = System.Environment.GetEnvironmentVariable("ossHttp");
#endif
                return ossHttp;
            }
        }
        /// <summary>
        /// OssBucket
        /// </summary>
        public static string OssBucket
        {
            get
            {
#if DEBUG
                var ossBucket = System.Environment.GetEnvironmentVariable("ossBucket", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var ossBucket = System.Environment.GetEnvironmentVariable("ossBucket");
#endif
                return ossBucket;
            }
        }
        /// <summary>
        /// ossUrl
        /// </summary>
        public static string OssUrl
        {
            get
            {
#if DEBUG
                var ossUrl = System.Environment.GetEnvironmentVariable("ossUrl", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var ossUrl = System.Environment.GetEnvironmentVariable("ossUrl");
#endif
                return ossUrl;
            }
        }
        /// <summary>
        /// OssDir
        /// </summary>
        public static string OssDir
        {
            get
            {
#if DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossDir", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossDir");
#endif
                return ossDir;
            }
        }
    }
    #endregion
}
