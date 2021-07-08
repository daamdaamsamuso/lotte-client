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


    public class MenuDIDManager : DatabaseManager
    {
        public MenuDIDManager(string server)
            : base(server)
        {
        }

        #region Update
        public bool UpdateNewMenuInfo(NewMenuInfoRaw infoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.NVarChar, "ID", infoRaw.ID, 200);
                    parameterValues.Add(SqlDbType.NVarChar, "FileName", infoRaw.FileName, 200);
                    parameterValues.Add(SqlDbType.BigInt, "FileSize", infoRaw.FileSize);
                    parameterValues.Add(SqlDbType.NVarChar, "LocalFileName", infoRaw.LocalFileName, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_NewMenuScheduleInfo_UPDATE", parameterValues))
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

        public bool UpdateContent(MenuDIDInfoRaw infoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ID", infoRaw.ID, 200);
                    parameterValues.Add(SqlDbType.VarChar, "P1FileName", infoRaw.P1FileName, 200);
                    parameterValues.Add(SqlDbType.VarChar, "P1LocalFileName", infoRaw.P1LocalFileName, 200);
                    parameterValues.Add(SqlDbType.VarChar, "P2FileName", infoRaw.P2FileName, 200);
                    parameterValues.Add(SqlDbType.VarChar, "P2LocalFileName", infoRaw.P2LocalFileName, 200);
                    parameterValues.Add(SqlDbType.Float, "RunningTime", infoRaw.RunningTime, 200);
                    parameterValues.Add(SqlDbType.Int, "P1FileSize", infoRaw.P1FileSize);
                    parameterValues.Add(SqlDbType.Int, "P2FileSize", infoRaw.P2FileSize);
                    parameterValues.Add(SqlDbType.Float, "P1FileRunningTime", infoRaw.P1FileRunningTime);
                    parameterValues.Add(SqlDbType.Float, "P2FileRunningTime", infoRaw.P2FileRunningTime);

                    using (var sdr = this.mssql.StoredProcedure("DID_MenuDIDInfo_UPDATE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion

        public bool InsertContentsInfo(MenuDIDInfoRaw infoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ID", infoRaw.ID, 200);
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", infoRaw.CinemaCode, 100);
                    parameterValues.Add(SqlDbType.VarChar, "ItemCode", infoRaw.ItemCode,10);
                    parameterValues.Add(SqlDbType.VarChar, "P1FileName", infoRaw.P1FileName,200);
                    parameterValues.Add(SqlDbType.VarChar, "P1LocalFileName", infoRaw.P1LocalFileName,200);
                    parameterValues.Add(SqlDbType.VarChar, "P2FileName", infoRaw.P2FileName,200);
                    parameterValues.Add(SqlDbType.VarChar, "P2LocalFileName", infoRaw.P2LocalFileName,200);
                    parameterValues.Add(SqlDbType.Float, "RunningTime", infoRaw.RunningTime,200);
                    parameterValues.Add(SqlDbType.VarChar, "Showing", infoRaw.Showing,1);
                    parameterValues.Add(SqlDbType.VarChar, "ReqID", infoRaw.ReqID, 200);
                    parameterValues.Add(SqlDbType.VarChar, "MenuDIDItem", infoRaw.MenuDIDItem, 200);
                    parameterValues.Add(SqlDbType.Int, "P1FileSize", infoRaw.P1FileSize);
                    parameterValues.Add(SqlDbType.Int, "P2FileSize", infoRaw.P2FileSize);
                    parameterValues.Add(SqlDbType.Float, "P1FileRunningTime", infoRaw.P1FileRunningTime);
                    parameterValues.Add(SqlDbType.Float, "P2FileRunningTime", infoRaw.P2FileRunningTime);
                    using (var sdr = this.mssql.StoredProcedure("DID_MenuDIDInfo_INSERT", parameterValues))
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

        public bool InsertNewMenuInfo(NewMenuInfoRaw infoRaw)
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
                    parameterValues.Add(SqlDbType.NVarChar, "LocalFileName", infoRaw.LocalFileName,200);
                    parameterValues.Add(SqlDbType.NVarChar, "RegID", infoRaw.RegID, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_NewMenuInfo_INSERT", parameterValues))
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

        public bool InsertNewMenuScheduleInfo(NewMenuScheduleInfoRaw infoRaw)
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
                    parameterValues.Add(SqlDbType.Int, "TotalRunningTime", infoRaw.TotalRunningTime);
                    parameterValues.Add(SqlDbType.Int, "Panel", infoRaw.Panel);
                    parameterValues.Add(SqlDbType.NVarChar, "RegID", infoRaw.RegID, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_NewMenuScheduleInfo_INSERT", parameterValues))
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

        public bool DeleteContentsInfo(string ID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ID", ID, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_MenuDIDInfo_DELETE", parameterValues))
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

        public bool DeleteNewMenuInfo(string ID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.NVarChar, "ID", ID, 200);
                    using (var sdr = this.mssql.StoredProcedure("DID_NewMenuInfo_DELETE", parameterValues))
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

        public bool DeleteNewMenuScheduleInfo(string CinemaCode,string DidItem)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", CinemaCode, 10);
                    parameterValues.Add(SqlDbType.VarChar, "DidItem", DidItem, 10);
                    using (var sdr = this.mssql.StoredProcedure("DID_NewMenuScheduleInfo_DELETE", parameterValues))
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

        public List<MenuDIDInfoRaw> GetItemByID(string ID)
        {
            List<MenuDIDInfoRaw> item = new List<MenuDIDInfoRaw>();
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList param = new StoredProcedureParameterValueList();
                    param.Add(SqlDbType.VarChar, "ID", ID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_MenuDIDItem_SELECT", param))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                MenuDIDInfoRaw mdr = new MenuDIDInfoRaw
                                {
                                    ID = sdr["ID"].ToString(),
                                    ItemCode = sdr["ItemCode"].ToString(),
                                    CinemaCode = sdr["CinemaCode"].ToString(),
                                    P1FileName = sdr["P1FileName"].ToString(),
                                    P1LocalFileName = sdr["P1LocalFileName"].ToString(),
                                    P2FileName = sdr["P2FileName"].ToString(),
                                    P2LocalFileName = sdr["P2LocalFileName"].ToString(),
                                    RunningTime = double.Parse(sdr["RunningTime"].ToString()),
                                    Showing = sdr["Showing"].ToString(),
                                    ReqID = sdr["ReqID"].ToString(),

                                    P1FileRunningTime = double.Parse(sdr["P1FileRunningTime"].ToString()),
                                    P2FileRunningTime = double.Parse(sdr["P2FileRunningTime"].ToString()),

                                    P1FileSize = Int32.Parse(sdr["P1FileSize"].ToString()),
                                    P2FileSize = Int32.Parse(sdr["P2FileSize"].ToString()),
                                    MenuDIDItem = sdr["MenuDIDItem"].ToString()
                                };
                                item.Add(mdr);
                            }
                        }
                    }

                }
            }
            catch
            {
            }
            return item;
        }

        public List<MenuDIDInfoRaw> GetContentList(int CinemaCode, string MenuDIDItem)
        {
            List<MenuDIDInfoRaw> contentList = new List<MenuDIDInfoRaw>();
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", CinemaCode, 10);
                    parameterValues.Add(SqlDbType.VarChar, "MenuDIDItem", MenuDIDItem, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_MenuDIDContentList_SELECT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                MenuDIDInfoRaw mdr = new MenuDIDInfoRaw
                                {
                                    ID = sdr["ID"].ToString(),
                                    ItemCode = sdr["ItemCode"].ToString(),
                                    CinemaCode = sdr["CinemaCode"].ToString(),
                                    P1FileName = sdr["P1FileName"].ToString(),
                                    P1LocalFileName = sdr["P1LocalFileName"].ToString(),
                                    P2FileName = sdr["P2FileName"].ToString(),
                                    P2LocalFileName = sdr["P2LocalFileName"].ToString(),
                                    RunningTime = double.Parse(sdr["RunningTime"].ToString()),
                                    Showing = sdr["Showing"].ToString(),
                                    ReqID = sdr["ReqID"].ToString(),
                                    MenuDIDItem = sdr["MenuDIDItem"].ToString()
                                };
                                contentList.Add(mdr);

                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return contentList;
        }

        public List<NewMenuInfoRaw> GetNewMenuInfo()
        {
            List<NewMenuInfoRaw> itemList = new List<NewMenuInfoRaw>();
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    using (var sdr = this.mssql.StoredProcedure("DID_NewMenuInfo_SELECT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                NewMenuInfoRaw mdr = new NewMenuInfoRaw
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

        public List<NewMenuScheduleInfoProcedure> GetNewMenuScheduleInfo(string CinemaCode)
        {
            List<NewMenuScheduleInfoProcedure> itemList = new List<NewMenuScheduleInfoProcedure>();
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", CinemaCode, 10);
                    using (var sdr = this.mssql.StoredProcedure("DID_NewMenuScheduleInfo_SELECT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                NewMenuScheduleInfoProcedure mdr = new NewMenuScheduleInfoProcedure
                                {
                                    ID = sdr["ID"].ToString(),
                                    OrderNum = int.Parse(sdr["OrderNum"].ToString()),
                                    CinemaCode = sdr["CinemaCode"].ToString(),
                                    DidItem = sdr["DidItem"].ToString(),
                                    RunningTime = int.Parse(sdr["RunningTime"].ToString()),
                                    TotalRunningTime = int.Parse(sdr["TotalRunningTime"].ToString()),
                                    Panel = int.Parse(sdr["Panel"].ToString()),
                                    RegID = sdr["RegID"].ToString(),
                                    LocalFileName = sdr["LocalFileName"].ToString(),
                                    FileSize = long.Parse(sdr["FileSize"].ToString())
                                    
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
