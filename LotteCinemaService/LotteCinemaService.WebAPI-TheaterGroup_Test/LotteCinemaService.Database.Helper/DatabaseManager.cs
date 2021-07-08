using LotteCinemaLibraries.Database;
using System;

namespace LotteCinemaService.Database.Helper
{
    public abstract class DatabaseManager
    {
        #region Variable

        protected MSSQL mssql;

        private string _didConnectionString;
        private string _lhsTestConnectionString = "server=10.51.241.245,7702;database=DID;uid=dykim;pwd=vinyli1@";

        #endregion

        public DatabaseManager(string connectionString)
        {
            this._didConnectionString = connectionString;
            this.mssql = new MSSQL();
        }

        public void DisConnection()
        {
            this.mssql.DisConnection();
        }

        public bool ConnectionDID()
        {
            return this.mssql.Connection(this._didConnectionString);
        }
    }
}