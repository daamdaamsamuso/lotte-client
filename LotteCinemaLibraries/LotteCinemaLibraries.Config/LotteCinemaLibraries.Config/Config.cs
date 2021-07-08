
namespace LotteCinemaLibraries.Config
{
    public class Config
    {
        // FTP
        //public static readonly string FTP_SERVER_NAME = "124.243.42.37:40051";
        public static readonly string FTP_SERVER_NAME = "124.243.42.39:40051";
        public static readonly string FTP_USER_NAME = "lhsftp";
        public static readonly string FTP_PASSWORD = "anqltmxm@LHS";


        //public static readonly string FTP_SERVER_NAME = "10.51.239.219";
        //public static readonly string FTP_USER_NAME = "ftpuser";
        //public static readonly string FTP_PASSWORD = "dpsdhTlftp!)!9";

        public static readonly string FTP_SERVER_NAME_TEST = "124.243.42.39:40051";
        public static readonly string FTP_USER_NAME_TEST = "lhsftp";
        public static readonly string FTP_PASSWORD_TEST = "anqltmxm@LHS";

        // TBA
        public static readonly string LOCAL_TBA_AD_PATH = "Contents\\AD";
        public static readonly string LOCAL_TBA_TRANSPARENT_AD_PATH = "Contents\\TransparentAD";
        public static readonly string LOCAL_TBA_SPECIAL_AD_PATH = "Contents\\SpecialAD";
        public static readonly string LOCAL_TBA_MOVIE_POSTER_PATH = "Contents\\Poster";

        public static readonly string FTP_TBA_AD_PATH = "DID\\ADVER\\TBA";
        public static readonly string FTP_TBA_TRANSPARENT_AD_PATH = "DID\\TBA\\TransparentAD";
        public static readonly string FTP_TBA_TRANSPARENTAlpha_AD_PATH = "DID\\TBA\\TransparentADAlpha";
        public static readonly string FTP_BIGTBA_AD_PATH = "DID\\TBA\\AD";
        public static readonly string FTP_TBA_SPECIAL_AD_PATH = "DID\\TBA\\SpecialAD";
        public static readonly string FTP_TBA_MOVIE_POSTER_PATH = "DID\\MEDIA";

        // NS
        public static readonly string LOCAL_NS_AD_PATH = "Contents\\AD";
        public static readonly string LOCAL_NS_SPECIAL_AD_PATH = "Contents\\SpecialAD";
        public static readonly string LOCAL_NS_MOVIE_POSTER_PATH = "Contents\\Poster";

        public static readonly string FTP_NS_AD_PATH = "DID\\NS\\AD";
        public static readonly string FTP_NS_SPECIAL_AD_PATH = "DID\\NS\\SpecialAD";
        public static readonly string FTP_NS_MOVIE_POSTER_PATH = "DID\\MEDIA";

        // TBA Info
        public static readonly string LOCAL_TBAINFO_AD_POSTER_PATH = "Contents\\AD_Poster";
        public static readonly string LOCAL_TBAINFO_BOXOFFICE_PATH = "Contents\\BoxOffice";
        public static readonly string LOCAL_TBAINFO_FORECAST_PATH = "Contents\\Forecast";
        public static readonly string LOCAL_TBAINFO_POSTER_PATH = "Contents\\Poster";
        public static readonly string LOCAL_TBAINFO_SKIN_PATH = "Contents\\Skin";

        public static readonly string FTP_TBAINFO_AD_BOXOFFICE_PATH = "DID\\ADVER\\BOXOFFICE";
        public static readonly string FTP_TBAINFO_AD_POSTER_PATH = "DID\\TBAInfo\\AD";
        public static readonly string FTP_TBAINFO_BOXOFFICE_PATH = "DID\\MEDIA";
        //public static readonly string FTP_TBAINFO_FORECAST_PATH = "DID\\MEDIA_Forecast";
        public static readonly string FTP_TBAINFO_FORECAST_PATH = "DID\\MEDIA";
        public static readonly string FTP_TBAINFO_SKIN_PATH = "DID\\TBAInfo\\Skin";

        // MultiCube
        public static readonly string LOCAL_MULTICUBE_MEDIA_PATH = "Contents\\MainAd";
        public static readonly string FTP_MULTICUBE_MEDIA_PATH = "DID\\MultiCube\\AD";

        // Digital Window
        public static readonly string LOCAL_DW_MEDIA_PATH = "Contents\\Media";
        public static readonly string LOCAL_DW_AD_PATH = "Contents\\AD";
        public static readonly string LOCAL_DW_SPECIAL_AD_PATH = "Contents\\SpecialAD";

        public static readonly string FTP_DW_MEDIA_PATH = "DID\\DigitalWindow\\Media";
        public static readonly string FTP_DW_AD_PATH = "DID\\DigitalWindow\\AD";
        public static readonly string FTP_DW_SPECIAL_AD_PATH = "DID\\DigitalWindow\\SpecialAD";

        // Digital Sign
        public static readonly string LOCAL_DS_MEDIA_PATH = "Contents\\Media";
        public static readonly string LOCAL_DS_SKIN_PATH = "Contents\\Skin";
        public static readonly string LOCAL_DS_RESOURCE_PATH = "Contents\\Resource";

        public static readonly string FTP_DS_MEDIA_PATH = "DID\\DigitalSign\\Media";
        public static readonly string FTP_DS_SKIN_PATH = "DID\\DigitalSign\\Skin";
        public static readonly string FTP_DS_RESOURCE_PATH = "DID\\DigitalSign\\Resource";

        // WelcomeFacade
        public static readonly string LOCAL_WF_MEDIA_PATH = "Contents\\Media";
        public static readonly string LOCAL_WF_AD_PATH = "Contents\\AD";
        public static readonly string LOCAL_WF_SPECIAL_AD_PATH = "Contents\\SpecialAD";
        public static readonly string LOCAL_WF_SKIN_PATH = "Contents\\Skin";

        public static readonly string FTP_WF_MEDIA_PATH = "DID\\WelcomeFacade\\Media";
        public static readonly string FTP_WF_AD_PATH = "DID\\WelcomeFacade\\AD";
        public static readonly string FTP_WF_SPECIAL_AD_PATH = "DID\\WelcomeFacade\\SpecialAD";
        public static readonly string FTP_WF_SKIN_PATH = "DID\\WelcomeFacade\\Skin";

        // Digital Gallery
        public static readonly string LOCAL_DG_MEDIA_PATH = "Contents\\Media";
        public static readonly string LOCAL_DG_AD_PATH = "Contents\\AD";
        public static readonly string LOCAL_DG_SPECIAL_AD_PATH = "Contents\\SpecialAD";
        public static readonly string LOCAL_DG_SKIN_PATH = "Contents\\Skin";

        public static readonly string FTP_DG_MEDIA_PATH = "DID\\DigitalGallery\\Media";
        public static readonly string FTP_DG_AD_PATH = "DID\\DigitalGallery\\AD";
        public static readonly string FTP_DG_SPECIAL_AD_PATH = "DID\\DigitalGallery\\SpecialAD";
        public static readonly string FTP_DG_SKIN_PATH = "DID\\DigitalGallery\\Skin";

        // KIOSK
        public static readonly string LOCAL_KO_AD_PATH = "Contents\\AD";
        public static readonly string FTP_KO_AD_PATH = "DID\\DigitalGallery\\AD";

        // Digital Curtain
        public static readonly string LOCAL_DC_MEDIA_PATH = "Contents\\Media";
        public static readonly string LOCAL_DC_EVENT_PATH = "Contents\\Event";
        public static readonly string LOCAL_DC_AD_PATH = "Contents\\AD";
        public static readonly string LOCAL_DC_SPECIAL_AD_PATH = "Contents\\SpecialAD";
        public static readonly string LOCAL_DC_SKIN_PATH = "Contents\\Skin";

        public static readonly string FTP_DC_MEDIA_PATH = "DID\\DigitalCurtain\\Media";
        public static readonly string FTP_DC_EVENT_PATH = "DID\\DigitalCurtain\\Event";
        public static readonly string FTP_DC_AD_PATH = "DID\\DigitalCurtain\\AD";
        public static readonly string FTP_DC_SPECIAL_AD_PATH = "DID\\DigitalCurtain\\SpecialAD";
        public static readonly string FTP_DC_SKIN_PATH = "DID\\DigitalCurtain\\Skin";

        // TMB
        public static readonly string LOCAL_TMB_MAINAD_PATH = "Contents\\MainAD";
        public static readonly string LOCAL_TMB_SUBAD_PATH = "Contents\\SubAD";
        public static readonly string LOCAL_TMB_SPECIALAD_PATH = "Contents\\SpecialAD";
        public static readonly string LOCAL_TMB_COMMINGMOVIE_PATH = "Contents\\CommingMovie";
        public static readonly string LOCAL_TMB_RECOMMENDMOVIE_PATH = "Contents\\RecommendMovie";
        public static readonly string LOCAL_TMB_EVNT_PATH = "Contents\\Event";
        public static readonly string LOCAL_TMB_EVNTMINI_PATH = "Contents\\EventMini";
        public static readonly string LOCAL_TMB_SKIN_PATH = "Contents\\Skin";

        public static readonly string FTP_TMB_MAINAD_PATH = "DID\\TMB\\AD";
        public static readonly string FTP_TMB_SUBAD_PATH = "DID\\TMB\\Sub\\AD";
        public static readonly string FTP_TMB_SPECIALAD_PATH = "DID\\TMB\\SpecialAD";
        public static readonly string FTP_TMB_COMMINGMOVIE_PATH = "DID\\TMB\\CommingMovie";
        public static readonly string FTP_TMB_RECOMMENDMOVIE_PATH = "DID\\MEDIA";
        public static readonly string FTP_TMB_EVNT_PATH = "DID\\TMB\\Event";
        public static readonly string FTP_TMB_EVNTMINI_PATH = "DID\\TMB\\EventMini";
        public static readonly string FTP_TMB_SKIN_PATH = "DID\\TMB\\Skin";

        // IS
        public static readonly string LOCAL_IS_SPECIAL_IMAGE_PATH = "Contents\\SpecialImage";
        public static readonly string LOCAL_IS_EVENT_IMAGE_PATH = "Contents\\Event";
        public static readonly string LOCAL_IS_SKIN_PATH = "Contents\\Skin";

        public static readonly string FTP_IS_SPECIAL_IMAGE_PATH = "DID\\IS\\SpecialImage";
        public static readonly string FTP_IS_EVENT_IMAGE_PATH = "DID\\IS\\Event";
        public static readonly string FTP_IS_SKIN_PATH = "DID\\IS\\Skin";

        // ES
        public static readonly string LOCAL_ES_EVENT_IMAGE_PATH = "Contents\\Event";

        public static readonly string FTP_ES_EVENT_IMAGE_PATH = "DID\\EntranceSystem\\Event";

        // Movie Contents Uploader
        public static readonly string FTP_MOVIE_CONTENTS_UPLOADER = "DID\\MEDIA";
    }
}