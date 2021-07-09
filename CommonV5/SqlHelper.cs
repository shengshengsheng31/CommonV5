using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    public class SqlHelper
    {
        public SqlSugarClient Db { get; set; }
        public SqlHelper(string connectionString, DbType dbType)
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.PostgreSQL,
                IsAutoCloseConnection = true,
            });
        }
        
    }
}
