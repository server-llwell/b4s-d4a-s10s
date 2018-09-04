using ACBC.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    public class DemoBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.DemoApi;
        }

        public object Do_Login(BaseApi baseApi)
        {
            DemoLoginParam demoLoginParam = JsonConvert.DeserializeObject<DemoLoginParam>(baseApi.param.ToString());
            if (demoLoginParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            var token = Global.TokenIntoRedis(baseApi.code);
            
            return new DemoLoginResult { token = token };
        }
    }

    public class DemoLoginParam
    {
        public string code;
    }

    public class DemoLoginResult
    {
        public string token;
    }
}
