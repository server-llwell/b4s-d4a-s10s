using ACBC.Buss;
using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class OpenDao
    {
        public User GetUser(string openid)
        {
            User user = null;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(OpenSqls.SELECT_STAFF_USER, openid);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null &&  dt.Rows.Count == 1)
            {
                user = new User
                {
                    openid = dt.Rows[0]["OPENID"].ToString(),
                    userId = dt.Rows[0]["STAFF_USER_ID"].ToString(),
                    userImg = dt.Rows[0]["USER_IMG"].ToString(),
                    userName = dt.Rows[0]["USER_NAME"].ToString(),
                    userType = dt.Rows[0]["USER_TYPE"].ToString()
                };
                
            }
            return user;
        }

        public bool GetUserCode(string userCode)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(OpenSqls.SELECT_STAFF_USER_BY_CODE, userCode);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count == 1)
            {
                return true;

            }
            return false;
        }

        public bool UserReg(UserRegParam userRegParam, string openID)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(OpenSqls.UPDATE_WXAPP_STAFF_USER,
                userRegParam.nickName,
                userRegParam.avatarUrl,
                openID,
                userRegParam.userCode);
            string sqlInsert = builder.ToString();

            return DatabaseOperationWeb.ExecuteDML(sqlInsert);
        }

        public class OpenSqls
        {
            public const string SELECT_STAFF_USER = ""
                + "SELECT * "
                + "FROM T_WXAPP_STAFF_USER "
                + "WHERE OPENID = '{0}' ";
            public const string SELECT_STAFF_USER_BY_CODE = ""
                + "SELECT * "
                + "FROM T_WXAPP_STAFF_USER "
                + "WHERE USER_CODE = '{0}' "
                + "AND OPENID IS NULL";
            public const string UPDATE_WXAPP_STAFF_USER = ""
                + "UPDATE T_WXAPP_STAFF_USER "
                + "SET USER_NAME = '{0}', "
                + "USER_IMG = '{1}', "
                + "OPENID = '{2}' "
                + "WHERE USER_CODE = '{3}' "
                + "AND OPENID IS NULL";
        }
    }
}
