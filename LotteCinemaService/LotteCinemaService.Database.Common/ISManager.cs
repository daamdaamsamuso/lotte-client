using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Database.Manager
{
    public class ISManager : DatabaseManager
    {
        public ISManager(string server)
            : base(server)
        {

        }

        public List<ISSpecialImageInfoProcedure> GetISSpecialImageInfoList(string theater, string itemID)
        {
            List<ISSpecialImageInfoProcedure> list = new List<ISSpecialImageInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var sdr = this.mssql.StoredProcedure("DID_ISSpecialImageInfoList_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ISSpecialImageInfoProcedure item = new ISSpecialImageInfoProcedure
                            {
                                ISID = DatabaseUtil.TryConvertToString(sdr["ISID"]),
                                BeginDate = DatabaseUtil.TryConvertToString(sdr["BeginDate"]),
                                EndDate = DatabaseUtil.TryConvertToString(sdr["EndDate"]),
                                ContentsID = DatabaseUtil.TryConvertToInteger(sdr["ContentsID"]),
                                FileName = DatabaseUtil.TryConvertToString(sdr["FileName"]),
                                FileType = DatabaseUtil.TryConvertToString(sdr["FileType"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(sdr["ContentsType"]),
                            };

                            list.Add(item);
                        }

                        sdr.Close();
                    }

                    DisConnection();
                }
            }

            return list;
        }

        public List<ISSpecialImageInfoRaw> GetISSpecialImageInfo(string theater, string itemID, string begin, string end)
        {
            List<ISSpecialImageInfoRaw> list = new List<ISSpecialImageInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.VarChar, "BeginDate",begin, 20);
                parameterValues.Add(SqlDbType.VarChar, "EndDate", end, 20);

                using (var sdr = this.mssql.StoredProcedure("DID_ISSpecialImageInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ISSpecialImageInfoRaw item = new ISSpecialImageInfoRaw
                            {
                                Title = DatabaseUtil.TryConvertToString(sdr["Title"]),
                                ISID = DatabaseUtil.TryConvertToString(sdr["ISID"]),
                                BeginDate = DatabaseUtil.TryConvertToString(sdr["BeginDate"]),
                                EndDate = DatabaseUtil.TryConvertToString(sdr["EndDate"]),
                              ItemID = DatabaseUtil.TryConvertToString(sdr["ItemID"])
                            };

                            list.Add(item);
                        }

                        sdr.Close();
                    }

                    DisConnection();
                }
            }

            return list;
        }

        public List<ISMenuInfoRaw> GetISMenuInfoList(string theater, string itemID)
        {
            List<ISMenuInfoRaw> list = new List<ISMenuInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var sdr = this.mssql.StoredProcedure("DID_ISMenuInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ISMenuInfoRaw item = new ISMenuInfoRaw
                            {
                                ISID = DatabaseUtil.TryConvertToString(sdr["ISID"]),
                                Theater = DatabaseUtil.TryConvertToString(sdr["Theater"]),
                                ItemID = DatabaseUtil.TryConvertToString(sdr["ItemID"]),
                                ISName = DatabaseUtil.TryConvertToString(sdr["ISName"]),
                                HomeMainType = DatabaseUtil.TryConvertToString(sdr["HomeMainType"]),
                                FloorPageVisible = DatabaseUtil.TryConvertCharToBool(sdr["FloorPageVisible"]),
                                NoticePageVisible = DatabaseUtil.TryConvertCharToBool(sdr["NoticePageVisible"]),
                                IdleInterval = DatabaseUtil.TryConvertToInteger(sdr["IdleInterval"]),
                                NoticeExposureTime = DatabaseUtil.TryConvertToInteger(sdr["NoticeExposureTime"]),
                                Location = DatabaseUtil.TryConvertToString(sdr["Location"]),
                                RegDate = DatabaseUtil.TryConvertToDateTime(sdr["RegDate"]),
                                RegID = DatabaseUtil.TryConvertToString(sdr["RegID"]),
                                UpdateDate = DatabaseUtil.TryConvertToDateTime(sdr["UpdateDate"]),
                                UpdateRegID = DatabaseUtil.TryConvertToString(sdr["UpdateRegID"]),
                            };

                            list.Add(item);
                        }

                        sdr.Close();
                    }

                    DisConnection();
                }
            }

            return list;
        }

        public List<ISMenuInfoRaw> GetISMenuInfoList(string theater)
        {
            List<ISMenuInfoRaw> list = new List<ISMenuInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID",DBNull.Value, 3);

                using (var sdr = this.mssql.StoredProcedure("DID_ISMenuInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ISMenuInfoRaw item = new ISMenuInfoRaw
                            {
                                ISID = DatabaseUtil.TryConvertToString(sdr["ISID"]),
                                Theater = DatabaseUtil.TryConvertToString(sdr["Theater"]),
                                ItemID = DatabaseUtil.TryConvertToString(sdr["ItemID"]),
                                ISName = DatabaseUtil.TryConvertToString(sdr["ISName"]),
                                HomeMainType = DatabaseUtil.TryConvertToString(sdr["HomeMainType"]),
                                FloorPageVisible = DatabaseUtil.TryConvertCharToBool(sdr["FloorPageVisible"]),
                                NoticePageVisible = DatabaseUtil.TryConvertCharToBool(sdr["NoticePageVisible"]),
                                IdleInterval = DatabaseUtil.TryConvertToInteger(sdr["IdleInterval"]),
                                NoticeExposureTime = DatabaseUtil.TryConvertToInteger(sdr["NoticeExposureTime"]),
                                Location = DatabaseUtil.TryConvertToString(sdr["Location"]),
                                RegDate = DatabaseUtil.TryConvertToDateTime(sdr["RegDate"]),
                                RegID = DatabaseUtil.TryConvertToString(sdr["RegID"]),
                                UpdateDate = DatabaseUtil.TryConvertToDateTime(sdr["UpdateDate"]),
                                UpdateRegID = DatabaseUtil.TryConvertToString(sdr["UpdateRegID"]),
                            };


                            if ((ItemID)int.Parse(item.ItemID.ToString()) == ItemID.ISAll)
                            {
                                continue;
                            }


                            list.Add(item);
                        }

                        sdr.Close();
                    }

                    DisConnection();
                }
            }

            return list;
        }

        public bool InsertISSpecialImage(ISSpecialImageInfoRaw item)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "Title", item.Title, 200);
                    parameterValues.Add(SqlDbType.VarChar, "ISID", item.ISID, 20);
                    parameterValues.Add(SqlDbType.Char, "ItemID", item.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", item.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", item.BeginDate, 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", item.EndDate, 20);

                    using (var sdr = this.mssql.StoredProcedure("DID_ISSpecialImageInfo_INSERT", parameterValues))
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

        public bool InsertMenuInfoList(List<ISMenuInfoRaw> items)
        {
            try
            {
                if (ConnectionDID())
                {
                    foreach (var item in items)
                    {
                        StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                        parameterValues.Add(SqlDbType.VarChar, "ISID", item.ISID, 13);
                        parameterValues.Add(SqlDbType.VarChar, "Theater", item.Theater, 10);
                        parameterValues.Add(SqlDbType.Char, "ItemID", item.ItemID, 3);
                        parameterValues.Add(SqlDbType.VarChar, "ISName", item.ISName, 50);
                        parameterValues.Add(SqlDbType.Char, "HomeMainType", item.HomeMainType, 2);
                        parameterValues.Add(SqlDbType.Char, "FloorPageVisible", item.FloorPageVisible == true ? "1" : "0", 1);
                        parameterValues.Add(SqlDbType.Char, "NoticePageVisible", item.NoticePageVisible == true ? "1" : "0", 1);
                        parameterValues.Add(SqlDbType.Int, "IdleInterval", item.IdleInterval);
                        parameterValues.Add(SqlDbType.Int, "NoticeExposureTime", item.NoticeExposureTime);
                        parameterValues.Add(SqlDbType.VarChar, "Location", item.Location, 100);
                        parameterValues.Add(SqlDbType.VarChar, "UpdateRegID", item.UpdateRegID, 100);

                        using (var sdr = this.mssql.StoredProcedure("DID_MenuInfoList_INSERT", parameterValues))
                        {
                            if (sdr != null)
                            {
                                sdr.Close();
                            }
                        }
                    }
                    DisConnection();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSpecialImageInfo(string isID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ISID", isID, 20);


                    using (var sdr = this.mssql.StoredProcedure("DID_SpecialImageInfo_DELETE", parameterValues))
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