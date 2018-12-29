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
                    userId = dt.Rows[0]["USER_ID"].ToString(),
                    userImg = dt.Rows[0]["USER_IMG"].ToString(),
                    userName = dt.Rows[0]["USER_NAME"].ToString(),
                    userType = dt.Rows[0]["USER_TYPE"].ToString()
                };
                
            }
            return user;
        }

        public class OpenSqls
        {
            public const string SELECT_STAFF_USER = ""
                + "SELECT * "
                + "FROM T_WXAPP_STAFF_USER "
                + "WHERE OPENID = '{0}' ";
        }
    }
}
