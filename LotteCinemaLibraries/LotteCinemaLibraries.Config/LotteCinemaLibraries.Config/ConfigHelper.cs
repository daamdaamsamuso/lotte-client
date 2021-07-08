using System.IO;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaLibraries.Config
{
    public class ConfigHelper
    {
        public static ConfigFile GetTBAAdFilePath(string adCode, int index, string extension)
        {
            var format = "{0}\\{1}\\{1}_{2}.{3}";

            ConfigFile result = new ConfigFile
            {
                FtpFilePath = string.Format(format, Config.FTP_TBA_AD_PATH, adCode, index, extension),
                LocalFilePath = string.Format(format, Config.LOCAL_TBA_AD_PATH, adCode, index, extension)
            };

            return result;
        }

        public static ConfigFile GetTBAInfoAdFilePath(string adCode, int adverType, string extension, bool isBoxOffice)
        {
            int fileType;
            string dirPath;

            if (isBoxOffice)
            {
                fileType = 1;
                dirPath = Config.FTP_TBAINFO_AD_BOXOFFICE_PATH;
            }
            else
            {
                fileType = adverType == 1 ? 2 : 3;
                dirPath = Config.FTP_TBA_AD_PATH;
            }

            var format = "{0}\\{1}\\{1}_{2}.{3}";

            ConfigFile result = new ConfigFile
            {
                FtpFilePath = string.Format(format, dirPath, adCode, fileType, extension),
                LocalFilePath = string.Format(format, Config.LOCAL_TBA_AD_PATH, adCode, fileType, extension)
            };

            return result;
        }

        public static ConfigFile GetTBAInfoContentFilePath(string movieCode, string extension, bool isBoxOffice)
        {
            var format = "{0}\\{1}\\{2}.{3}";

            var ftpDirPath = isBoxOffice ? Config.FTP_TBAINFO_BOXOFFICE_PATH : Config.FTP_TBAINFO_FORECAST_PATH;
            var localDirPath = isBoxOffice ? Config.LOCAL_TBAINFO_BOXOFFICE_PATH : Config.LOCAL_TBAINFO_FORECAST_PATH;

            ConfigFile result = new ConfigFile
            {
                FtpFilePath = string.Format(format, ftpDirPath, movieCode, movieCode + "_303_1", extension),
                LocalFilePath = string.Format(format, localDirPath, movieCode, movieCode + "_303_1", extension)
            };

            return result;
        }

        public static ConfigFile GetTBAInfoPosterFilePath(string movieCode, bool isBoxOffice)
        {
            var ftpDirPath = isBoxOffice ? Config.FTP_TBAINFO_BOXOFFICE_PATH : Config.FTP_TBAINFO_FORECAST_PATH;

            ConfigFile result = new ConfigFile
            {
                FtpFilePath = string.Format("{0}\\{1}\\{1}_701_1.jpg", ftpDirPath, movieCode),
                LocalFilePath = string.Format("{0}\\{1}_701_1.jpg", Config.LOCAL_TBAINFO_POSTER_PATH, movieCode)
            };

            return result;
        }

        public static ConfigFile GetFilePath(string dirName, string fileName, ItemID id, ContentsType type)
        {
            switch (id)
            {
                case ItemID.TBA:
                    return GetTBAFilePath(dirName, fileName, type);

                case ItemID.TBAInfo:
                    return GetTBAInfoFilePath(dirName, fileName, type);

                case ItemID.TMB:
                    return GetTMBFilePath(dirName, fileName, type);

                case ItemID.DigitalWindow:
                    return GetDigitalWindowFilePath(dirName, fileName, type);

                case ItemID.DigitalSign:
                    return GetDigitalSignFilePath(dirName, fileName, type);

                case ItemID.MultiCube:
                    return GetMultiCubeFilePath(dirName, fileName, type);

                case ItemID.ISAll:
                case ItemID.IS01:
                case ItemID.IS02:
                case ItemID.IS03:
                case ItemID.IS04:
                case ItemID.IS05:
                case ItemID.IS06:
                case ItemID.IS07:
                case ItemID.ISTochless01:
                case ItemID.ISTochless02:
                    return GetISFilePath(dirName, fileName, type);
            }

            return new ConfigFile();
        }

        private static ConfigFile GetTBAFilePath(string dirName, string fileName, ContentsType type)
        {
            ConfigFile result = new ConfigFile();

            switch (type)
            {
                case ContentsType.Adver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TBA_AD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_BIGTBA_AD_PATH, dirName, fileName);
                    break;

                case ContentsType.SpecialAdver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TBA_SPECIAL_AD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TBA_SPECIAL_AD_PATH, dirName, fileName);
                    break;

                case ContentsType.Poster:
                    var posterFileName = string.Format("{0}_701_1.jpg", dirName);
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TBA_MOVIE_POSTER_PATH, posterFileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TBA_MOVIE_POSTER_PATH, dirName, posterFileName);
                    break;

                case ContentsType.TransparentAd:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TBA_TRANSPARENT_AD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TBA_TRANSPARENT_AD_PATH, dirName, fileName);
                    break;
            }

            return result;
        }

        private static ConfigFile GetTBAInfoFilePath(string dirName, string fileName, ContentsType type)
        {
            ConfigFile result = new ConfigFile();

            switch (type)
            {
                case ContentsType.Poster:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TBAINFO_AD_POSTER_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TBAINFO_AD_POSTER_PATH, dirName, fileName);
                    break;

                case ContentsType.Skin:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TBAINFO_SKIN_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TBAINFO_SKIN_PATH, dirName, fileName);
                    break;
            }

            return result;
        }

        private static ConfigFile GetTMBFilePath(string dirName, string fileName, ContentsType type)
        {
            ConfigFile result = new ConfigFile();

            switch (type)
            {
                case ContentsType.Adver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TMB_MAINAD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TMB_MAINAD_PATH, dirName, fileName);
                    break;

                case ContentsType.SpecialAdver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TMB_SPECIALAD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TMB_SPECIALAD_PATH, dirName, fileName);
                    break;

                case ContentsType.SubAdver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TMB_SUBAD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TMB_SUBAD_PATH, dirName, fileName);
                    break;

                case ContentsType.RecommendMovie:
                    fileName = string.Format("{0}_701_1.jpg", dirName);
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TMB_RECOMMENDMOVIE_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TMB_RECOMMENDMOVIE_PATH, dirName, fileName);
                    break;

                case ContentsType.Event:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TMB_EVNT_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TMB_EVNT_PATH, dirName, fileName);
                    break;

                case ContentsType.Skin:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_TMB_SKIN_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_TMB_SKIN_PATH, dirName, fileName);
                    break;
            }

            return result;
        }

        private static ConfigFile GetDigitalWindowFilePath(string dirName, string fileName, ContentsType type)
        {
            ConfigFile result = new ConfigFile();

            switch (type)
            {
                case ContentsType.Adver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_DW_AD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_DW_AD_PATH, dirName, fileName);
                    break;

                case ContentsType.SpecialAdver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_DW_SPECIAL_AD_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_DW_SPECIAL_AD_PATH, dirName, fileName);
                    break;

                case ContentsType.Media:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_DW_MEDIA_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_DW_MEDIA_PATH, dirName, fileName);
                    break;
            }

            return result;
        }

        private static ConfigFile GetDigitalSignFilePath(string dirName, string fileName, ContentsType type)
        {
            ConfigFile result = new ConfigFile();

            switch (type)
            {
                case ContentsType.Skin:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_DS_SKIN_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_DS_SKIN_PATH, dirName, fileName);
                    break;

                case ContentsType.Notice: 
                    result.LocalFilePath = Path.Combine(Config.LOCAL_DS_RESOURCE_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_DS_RESOURCE_PATH, dirName, fileName);
                    break;
            }

            return result;
        }

        private static ConfigFile GetMultiCubeFilePath(string dirName, string fileName, ContentsType type)
        {
            ConfigFile result = new ConfigFile();

            switch (type)
            {
                case ContentsType.Adver:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_MULTICUBE_MEDIA_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_MULTICUBE_MEDIA_PATH, dirName, fileName);
                    break;
            }

            return result;
        }

        private static ConfigFile GetISFilePath(string dirName, string fileName, ContentsType type)
        {
            ConfigFile result = new ConfigFile();

            switch (type)
            {
                case ContentsType.Image:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_IS_SPECIAL_IMAGE_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_IS_SPECIAL_IMAGE_PATH, dirName, fileName);
                    break;
                case ContentsType.Event:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_IS_EVENT_IMAGE_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_IS_EVENT_IMAGE_PATH, dirName, fileName);

                    break;
                case ContentsType.Skin:
                    result.LocalFilePath = Path.Combine(Config.LOCAL_IS_SKIN_PATH, dirName, fileName);
                    result.FtpFilePath = Path.Combine(Config.FTP_IS_SKIN_PATH, dirName, fileName);
                    break;
            }

            return result;
        }
    }
}