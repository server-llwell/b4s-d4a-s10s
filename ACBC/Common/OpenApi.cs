using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Common
{
    /// <summary>
    /// API类型分组
    /// </summary>
    public enum ApiType
    {
        OpenApi,
        UploadApi,
        DashboardApi,
    }

    public enum CheckType
    {
        Open,
        Token,
        OpenToken,
        Sign,
    }

    public enum InputType
    {
        Header,
        Body,
    }

    public abstract class BaseApi
    {
        public string appId;
        public string lang;
        public string code;
        public string method;
        public string token;
        public string sign;
        public string nonceStr;
        public object param;

        public abstract CheckType GetCheckType();
        public abstract ApiType GetApiType();
        public abstract InputType GetInputType();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{2}; method:{0}; param:{1}", method, param, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            string rets = builder.ToString();
            return rets;
        }
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

        public override InputType GetInputType()
        {
            return InputType.Header;
        }

        public override ApiType GetApiType()
        {
            return ApiType.UploadApi;
        }

    }

    /// <summary>
    /// 完全开放
    /// </summary>
    public class OpenApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.Open;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.OpenApi;
        }
    }

    /// <summary>
    /// 完全开放
    /// </summary>
    public class DashboardApi : BaseApi
    {
        public override CheckType GetCheckType()
        {
            return CheckType.OpenToken;
        }

        public override InputType GetInputType()
        {
            return InputType.Body;
        }

        public override ApiType GetApiType()
        {
            return ApiType.DashboardApi;
        }
    }
}
