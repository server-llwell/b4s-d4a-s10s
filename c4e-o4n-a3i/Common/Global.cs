using ACBC.Buss;
using ACBC.Dao;
using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class Global
    {
        public const string ROUTE_PX = "openapi";

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

        /// <summary>
        /// OssDirOrder
        /// </summary>
        public static string OssDirOrder
        {
            get
            {
#if DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossDirOrder", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossDirOrder");
#endif
                return ossDir;
            }
        }

        /// <summary>
        /// ossB2BGoods
        /// </summary>
        public static string ossB2BGoods
        {
            get
            {
#if DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossB2BGoods", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossB2BGoods");
#endif
                return ossDir;
            }
        }

        /// <summary>
        /// ossB2BGoodsNum
        /// </summary>
        public static string ossB2BGoodsNum
        {
            get
            {
#if DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossB2BGoodsNum", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
                var ossDir = System.Environment.GetEnvironmentVariable("ossB2BGoodsNum");
#endif
                return ossDir;
            }
        }

#endregion
    }
}
