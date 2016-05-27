using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NordicArenaTournament.ErrorHandling
{
    /// <summary>
    /// Takk til http://www.codeproject.com/Messages/4281109/Simpler-appraoch.aspx
    /// </summary>
    public class CustomElmahSqlErrorLog : Elmah.SqlErrorLog
    {
        protected string ConnectionStringName;
        public CustomElmahSqlErrorLog(IDictionary config)
            : base(config)
        {
            ConnectionStringName = (string)config["connectionStringName"];
        }

        public override string ConnectionString
        {
            get 
            { 
                return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString; 
            }
        }
    }
}