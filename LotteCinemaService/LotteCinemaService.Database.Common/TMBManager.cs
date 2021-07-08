using System.Collections.Generic;
using System.Data;
using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common.Raw;
using System;

namespace LotteCinemaService.Database.Manager
{
    public class TMBManager : DatabaseManager
    {
        public TMBManager(string server)
            : base(server)
        {
        }


        public bool UpdateSpecialStatus(bool status)
        {
            try
            {
                if (ConnectionDID())
                {
                    var parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "IsFull", (status == true) ? "1" : "0");

                    using (var sdr = this.mssql.StoredProcedure("DID_TMBSpecialType_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}