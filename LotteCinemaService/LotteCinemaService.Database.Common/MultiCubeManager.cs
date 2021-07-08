using System;
using System.Collections.Generic;
using System.Data;
using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Database.Manager
{
   public class MultiCubeManager : DatabaseManager
    {
        public MultiCubeManager(string server)
            : base(server)
        {
        }

        public bool InsertContentsInfo(ContentsInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ContentsName", InfoRaw.ContentsName, 100);
                    parameterValues.Add(SqlDbType.Int, "Category", InfoRaw.Category);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", InfoRaw.BeginDate, 10);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", InfoRaw.EndDate, 10);
                    parameterValues.Add(SqlDbType.VarChar, "FileName", InfoRaw.FileName, 100);

                    if (!(InfoRaw.ContractID == 0))
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", InfoRaw.ContractID);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", DBNull.Value);
                    }
                    parameterValues.Add(SqlDbType.VarChar, "FileType", InfoRaw.FileType, 5);
                    parameterValues.Add(SqlDbType.BigInt, "FileSize", InfoRaw.FileSize);
                    parameterValues.Add(SqlDbType.VarChar, "GroupID", InfoRaw.GroupID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "ItemPositionID", InfoRaw.ItemPositionID, 2);
                    parameterValues.Add(SqlDbType.VarChar, "ContentsType", InfoRaw.ContentsType, 2);

                    using (var sdr = this.mssql.StoredProcedure("DID_ContentsInfo_INSERT", parameterValues))
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

        public bool InsertADInfo(AdInfoRaw InfoRaw)
        { 
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", InfoRaw.ID, 20);
                    if (InfoRaw.ContentsType == ContentsType.Adver)
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", InfoRaw.ContractID);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", DBNull.Value);
                    }
                    parameterValues.Add(SqlDbType.Char, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Title", InfoRaw.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", InfoRaw.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "ContentsType", ((int)InfoRaw.ContentsType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "LayoutType", ((int)InfoRaw.LayoutType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "SoundPosition", InfoRaw.SoundPosition.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", InfoRaw.BeginDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", InfoRaw.EndDate.ToString("yyyy-MM-dd HH:mm"), 20);

                    using (var sdr = this.mssql.StoredProcedure("DID_AdInfo_INSERT", parameterValues))
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
    
        public bool SetScheduleInfo(ScheduleInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ID", InfoRaw.ID, 20);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", InfoRaw.ContentsType, 2);
                    parameterValues.Add(SqlDbType.VarChar, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", InfoRaw.Theater, 10);
                    using (var sdr = this.mssql.StoredProcedure("DID_ScheduleInfo_INSERT", parameterValues))
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

        public List<ContractInfoProcedure> GetContractList(string itemCode, string AdvertiserName, string ContractID, string ContractName)
        {
            List<ContractInfoProcedure> contractList = new List<ContractInfoProcedure>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                
                if (ContractName == null || ContractName.Equals(string.Empty))
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractName", DBNull.Value);
                }
                else
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractName", ContractName, 100);
                }

                if (AdvertiserName == null || AdvertiserName.Equals(string.Empty))
                {
                    parameterValues.Add(SqlDbType.VarChar, "AdvertiserName", DBNull.Value);
                }
                else
                {
                    parameterValues.Add(SqlDbType.VarChar, "AdvertiserName", AdvertiserName, 50);
                }

                if (ContractID == null || ContractID.Equals(string.Empty))
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractID", DBNull.Value);
                }
                else
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractID", ContractID, 100);
                }
                
                using (var sdr = this.mssql.StoredProcedure("DID_ContractInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ContractInfoProcedure contractInfo = new ContractInfoProcedure
                            {
                                ContractID = int.Parse(sdr["ContractID"].ToString()).ToString(),
                                ContractName = sdr["ContractName"] as string,
                                AdvertiserID = int.Parse(sdr["AdvertiserID"].ToString()).ToString(),
                                ContractDate = sdr["ContractDate"] as string,
                                BeginDate = sdr["BeginDate"] as string,
                                EndDate = sdr["EndDate"] as string,
                                AdvertiserName = sdr["AdvertiserName"] as string
                            };
                            contractList.Add(contractInfo);
                        }
                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
            }
            return contractList;
        }
    }
}