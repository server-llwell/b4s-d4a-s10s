using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class AuthDao
    {
        /// <summary>
        /// 验证当前用户是否可调用api路径
        /// </summary>
        /// <param name="route">api路径</param>
        /// <param name="code">用户id</param>
        /// <returns></returns>
        public bool CheckAuth(string route, string code)
        {
            return true;
        }

        /// <summary>
        /// 获取用户的AppSecret
        /// </summary>
        /// <param name="code">用户id</param>
        /// <param name="appId">用户appId</param>
        /// <returns></returns>
        public string GetAccess(string code, string appId)
        {
            return "DemoAppSecret";
        }
    }

}
