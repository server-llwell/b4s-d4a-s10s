using Com.ACBC.Framework.Database;
using System;

namespace ACBC.Dao
{
    public class DBManager : IType
    {
        private DBType dbt;
        private string str = "";

        public DBManager()
        {
#if DEBUG
            var url = System.Environment.GetEnvironmentVariable("MysqlDBUrl", EnvironmentVariableTarget.User);
            var uid = System.Environment.GetEnvironmentVariable("MysqlDBUser", EnvironmentVariableTarget.User);
            var port = System.Environment.GetEnvironmentVariable("MysqlDBPort", EnvironmentVariableTarget.User);
            var passd = System.Environment.GetEnvironmentVariable("MysqlDBPassword", EnvironmentVariableTarget.User);
#endif
#if !DEBUG
            var url = System.Environment.GetEnvironmentVariable("MysqlDBUrl");
            var uid = System.Environment.GetEnvironmentVariable("MysqlDBUser");
            var port = System.Environment.GetEnvironmentVariable("MysqlDBPort");
            var passd = System.Environment.GetEnvironmentVariable("MysqlDBPassword");
            //this.str = "Database='llwell';Data Source='"+url+ "';User Id='" + uid + "';Password='" + passd + "';Character Set=utf8;port=" + port + ";";
#endif
            this.str = "Server=" + url
                     + ";Port=" + port
                     + ";Database=llwell;Uid=" + uid
                     + ";Pwd=" + passd
                     + ";CharSet=utf8; Encrypt=false;";
            Console.Write(this.str);
            this.dbt = DBType.Mysql;
        }

        public DBManager(DBType d, string s)
        {
            this.dbt = d;
            this.str = s;
        }

        public DBType getDBType()
        {
            return dbt;
        }

        public string getConnString()
        {
            return str;
        }

        public void setConnString(string s)
        {
            this.str = s;
        }
    }
}
