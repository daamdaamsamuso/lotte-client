using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Database.Manager
{
    public class MiniNoticeManager : DatabaseManager
    {
        public MiniNoticeManager(string server) : base(server)
        {}

        public bool InsertMiniNoticeInfo(MiniNoticeInfoRaw infoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.NVarChar, "ID", infoRaw.ID, 200);
                    parameterValues.Add(SqlDbType.NVarChar, "ItemCode", infoRaw.ItemCode, 10);
                    parameterValues.Add(SqlDbType.NVarChar, "FileName", infoRaw.FileName, 200);
                    parameterValues.Add(SqlDbType.BigInt, "FileSize", infoRaw.FileSize);
                    parameterValues.Add(SqlDbType.NVarChar, "LocalFileName", infoRaw.LocalFileName, 200);
                    parameterValues.Add(SqlDbType.NVarChar, "RegID", infoRaw.RegID, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_MiniNoticeInfo_INSERT", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
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

        public bool InsertMiniNoticeScheduleInfo(MiniNoticeScheduleInfoRaw infoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.NVarChar, "ID", infoRaw.ID, 200);
                    parameterValues.Add(SqlDbType.Int, "OrderNum", infoRaw.OrderNum);
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", infoRaw.CinemaCode, 10);
                    parameterValues.Add(SqlDbType.VarChar, "DidItem", infoRaw.DidItem, 10);
                    parameterValues.Add(SqlDbType.Int, "RunningTime", infoRaw.RunningTime);
                    parameterValues.Add(SqlDbType.NVarChar, "RegID", infoRaw.RegID, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_MiniNoticeScheduleInfo_INSERT", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
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

        public bool DeleteMiniNoticeInfo(string ID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.NVarChar, "ID", ID, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_MiniNoticeInfo_DELETE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
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

        public bool DeleteMiniNoticeScheduleInfo(string CinemaCode, string DidItem)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", CinemaCode, 10);
                    parameterValues.Add(SqlDbType.VarChar, "DidItem", DidItem, 10);
                    using (var sdr = this.mssql.StoredProcedure("DID_MiniNoticeScheduleInfo_DELETE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
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

        public List<MiniNoticeInfoRaw> GetMiniNoticeInfo()
        {
            List<MiniNoticeInfoRaw> itemList = new List<MiniNoticeInfoRaw>();
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    using (var sdr = this.mssql.StoredProcedure("DID_MiniNoticeInfo_SELECT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                MiniNoticeInfoRaw mdr = new MiniNoticeInfoRaw
                                {
                                    ID = sdr["ID"].ToString(),
                                    ItemCode = sdr["ItemCode"].ToString(),
                                    FileName = sdr["FileName"].ToString(),
                                    LocalFileName = sdr["LocalFileName"].ToString(),
                                    FileSize = long.Parse(sdr["FileSize"].ToString()),
                                    RegID = sdr["RegID"].ToString()
                                };
                                itemList.Add(mdr);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return itemList;
        }

        public List<MiniNoticeScheduleInfoProcedure> GetMiniNoticeScheduleInfo(string CinemaCode)
        {
            List<MiniNoticeScheduleInfoProcedure> itemList = new List<MiniNoticeScheduleInfoProcedure>();
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", CinemaCode, 10);
                    using (var sdr = this.mssql.StoredProcedure("DID_MiniNoticeScheduleInfo_SELECT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                MiniNoticeScheduleInfoProcedure mdr = new MiniNoticeScheduleInfoProcedure
                                {
                                    ID = sdr["ID"].ToString(),
                                    OrderNum = int.Parse(sdr["OrderNum"].ToString()),
                                    CinemaCode = sdr["CinemaCode"].ToString(),
                                    DidItem = sdr["DidItem"].ToString(),
                                    RunningTime = int.Parse(sdr["RunningTime"].ToString()),
                                    RegID = sdr["RegID"].ToString(),
                                    LocalFileName = sdr["LocalFileName"].ToString()
                                };
                                itemList.Add(mdr);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return itemList;
        }


    }
}
