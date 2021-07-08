using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Database.Manager
{
    public class DigitalGalleryManager : DatabaseManager
    {
        public DigitalGalleryManager(string server)
            : base(server)
        {

        }

        public bool DeleteMediaInfo(string id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", id, 20);


                    using (var sdr = this.mssql.StoredProcedure("DID_MediaInfo_DELETE", parameterValues))
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
