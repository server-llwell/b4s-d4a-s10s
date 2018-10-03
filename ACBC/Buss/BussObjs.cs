using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    #region Sys

    public class SessionUser
    {
        public string openid;
        public string checkPhone;
        public string checkCode;
        public string userType;
    }

    public class SmsCodeRes
    {
        public int error_code;
        public string reason;
    }

    public class WsPayState
    {
        public string userId;
        public string scanCode;
    }

    public class ExchangeRes
    {
        public string reason;
        public ExchangeResult result;
        public int error_code;
    }
    public class ExchangeResult
    {
        public string update;
        public List<string[]> list;
    }

    public enum ScanCodeType
    {
        Shop,
        User,
        Null,
    }

    #endregion

    #region Params

    public class LoginParam
    {
        public string code;
    }

    public class UserRegParam
    {
        public string checkCode;
        public string agentCode;
        public string avatarUrl;
        public string city;
        public string country;
        public string gender;
        public string language;
        public string nickName;
        public string province;
        public string phone;
        public string userType;
    }

    #endregion

    #region DaoObjs

    public class User
    {
        public string userName;
        public string userId;
        public string openid;
        public string userImg;
        public string phone;
        public string userType;
        public string scanCode;
        public string sex;
    }

    #endregion
}
