using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    /// <summary>
    /// API类型分组
    /// </summary>
    public enum ApiType
    {
        DemoApi,
        UploadApi,
    }

    public enum CheckType
    {
        Open,
        Token,
        Sign,
    }

    public abstract class BaseApi
    {
        public string appId;
        public string code;
        public string method;
        public string token;
        public string sign;
        public string nonceStr;
        public object param;

        public abstract CheckType GetCheckType();
        public abstract ApiType GetApiType();
    }

    /// <summary>
    /// Upload类API
    /// </summary>
    public class UploadApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Open;
        }

        public override ApiType GetApiType()
        {
            return ApiType.UploadApi;
        }

    }

    /// <summary>
    /// Demo类API
    /// </summary>
    public class DemoApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Sign;
        }

        public override ApiType GetApiType()
        {
            return ApiType.DemoApi;
        }

    }
}
