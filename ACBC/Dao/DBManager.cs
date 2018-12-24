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
            var url = System.Environment.GetEnvironmentVariable("MysqlDBUrl");
            var uid = System.Environment.GetEnvironmentVariable("MysqlDBUser");
            var port = System.Environment.GetEnvironmentVariable("MysqlDBPort");
            var passd = System.Environment.GetEnvironmentVariable("MysqlDBPassword");
            this.str = "Server=" + url
                     + ";Port=" + port
                     + ";Database=pg;Uid=" + uid
                     + ";Pwd=" + passd
                     + ";CharSet=utf8mb4; SslMode =none;";
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
