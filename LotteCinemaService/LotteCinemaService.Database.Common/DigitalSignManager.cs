using System.Collections.Generic;
using System.Data;
using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Database.Manager
{
    public class DigitalSignManager : DatabaseManager
    {
        public DigitalSignManager(string server) :
            base(server)
        {
        }

        public List<DigitalSignSkinProcedure> GetSkinInfoList(string theater)
        {
            List<DigitalSignSkinProcedure> list = new List<DigitalSignSkinProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)ContentsType.Skin).ToString("00"), 2);
                parameterValues.Add(SqlDbType.Char, "IsVisible", '1', 1);
                
                using (var reader = this.mssql.StoredProcedure("DID_DigitalSignInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            DigitalSignSkinProcedure item = new DigitalSignSkinProcedure
                            {
                                SkinID = DatabaseUtil.TryConvertToString(reader["SkinID"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                        reader.Dispose();
                    }
                }

                DisConnection();
            }

            return list;
        }

        public List<DigitalSignNoticeInfo> GetNoticeInfoList(string theater)
        {
            List<DigitalSignNoticeInfo> list = new List<DigitalSignNoticeInfo>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)ContentsType.Notice).ToString("00"), 2);
                parameterValues.Add(SqlDbType.Char, "IsVisible", '1', 1);

                using (var reader = this.mssql.StoredProcedure("DID_DigitalSignInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            DigitalSignNoticeInfo item = new DigitalSignNoticeInfo
                            {
                                Text = DatabaseUtil.TryConvertToString(reader["Text"]),
                                CharacterColor = DatabaseUtil.TryConvertToString(reader["CharacterColor"]),
                                CharacterBold = DatabaseUtil.TryConvertToString(reader["CharacterBold"]),
                                FontFamily = DatabaseUtil.TryConvertToString(reader["FontFamily"]),
                                IconFileName = DatabaseUtil.TryConvertToString(reader["IconFileName"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                        reader.Dispose();
                    }
                }

                DisConnection();
            }

            return list;
        }

        public bool DeleteNoticeInfo(string noticeID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", noticeID, 17);


                    using (var sdr = this.mssql.StoredProcedure("DID_DSNoticeInfo_DELETE", parameterValues))
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

        public DigitalSignInfoRaw GetNoticeInfoItem(string noticeID)
        {
            DigitalSignInfoRaw item = new DigitalSignInfoRaw();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "NoticeID", noticeID, 17);
                using (var reader = this.mssql.StoredProcedure("DID_DigitalSignInfoItem_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            item = new DigitalSignInfoRaw
                            {
                                NoticeID = DatabaseUtil.TryConvertToString(reader["NoticeID"]),
                                Text = DatabaseUtil.TryConvertToString(reader["Text"]),
                                CharacterColor = DatabaseUtil.TryConvertToString(reader["CharacterColor"]),
                                CharacterBold = DatabaseUtil.TryConvertToString(reader["CharacterBold"]),
                                FontFamily = DatabaseUtil.TryConvertToString(reader["FontFamily"]),
                                IconFileName = DatabaseUtil.TryConvertToString(reader["IconFileName"])
                            };
                        }

                        reader.Close();
                        reader.Dispose();
                    }
                }
                DisConnection();
            }
            return item;
        }
    }
}