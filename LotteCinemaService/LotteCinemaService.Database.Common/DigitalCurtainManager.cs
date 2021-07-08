using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using System.Collections.Generic;
using System.Data;

namespace LotteCinemaService.Database.Manager
{
    public class DigitalCurtainManager : DatabaseManager
    {
        public DigitalCurtainManager(string server)
            : base(server)
        {
        }

        public List<DigitalCurtainInfoRaw> GetDigitalCurtainInfo(ContentsType type)
        {
            List<DigitalCurtainInfoRaw> list = new List<DigitalCurtainInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)type).ToString("00"), 2);

                using (var reader = this.mssql.StoredProcedure("DID_DigitalCurtainInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        if (type == ContentsType.Weather || type == ContentsType.WeatherDefault)
                        {
                            while (reader.Read())
                            {
                                DigitalCurtainInfoRaw item = new DigitalCurtainInfoRaw
                                {
                                    WeatherType = (WeatherType)DatabaseUtil.TryConvertToInteger(reader["WeatherType"]),
                                    GroupID = DatabaseUtil.TryConvertToString(reader["GroupID"]),
                                    ContentsName = DatabaseUtil.TryConvertToString(reader["ContentsName"]),
                                    FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                    FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                    ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                    ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]),
                                    FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"])
                                };

                                list.Add(item);
                            }
                        }
                        else if (type == ContentsType.WeatherTime)
                        {
                            while (reader.Read())
                            {
                                DigitalCurtainInfoRaw item = new DigitalCurtainInfoRaw
                                {
                                    MorningBeginDate = DatabaseUtil.TryConvertToString(reader["MorningBeginDate"]),
                                    MorningEndDate = DatabaseUtil.TryConvertToString(reader["MorningEndDate"])
                                };

                                list.Add(item);
                            }
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }

        public bool UpdateDigitalCurtainDate(DigitalCurtainInfoRaw date)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "MorningBeginDate", date.MorningBeginDate, 5);
                    parameterValues.Add(SqlDbType.Char, "MorningEndDate", date.MorningEndDate, 5);
                    using (var sdr = this.mssql.StoredProcedure("DID_DigitalCurtainDATE_UPDATE", parameterValues))
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

        public bool UpdateDigitalCurtainWeather(DigitalCurtainInfoRaw weather)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "WeatherType", ((int)weather.WeatherType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.Char, "GroupID", weather.GroupID, 20);
                    using (var sdr = this.mssql.StoredProcedure("DID_DigitalCurtainContent_UPDATE", parameterValues))
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
