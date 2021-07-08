
namespace LotteCinemaService.WebAPI.Common.Models
{
    internal class Settings
    {
        internal const string SERVER_DID_CONNECTION_STRING = "server=10.119.1.22;database=DID;uid=lc_dsi;pwd=info0420!"; //"server=10.51.247.1;database=DID;uid=sa;pwd=Admin!@#$%";
        internal const string SERVER_DID_CONNECTION_STRING_TEST = "server=10.51.247.1;database=DID;uid=sa;pwd=Admin!@#$%";
        internal const string SERVER_LHS_CONNECTION_STRING_TEST = "server=10.51.241.245,7702;database=DID;uid=dykim;pwd=vinyli1@";

        /*
         LOTTE Ticket : 개발 - server=10.51.241.244;database=lcnew;uid=dykim;pwd=vinyli1@
                        운영 - server=10.51.242.10,7701;database=lcnew;uid=lces_select;pwd=select
         
         LOTTE Web    : 개발 - server=10.51.241.245,7702;database=LHS;uid=dykim;pwd=vinyli1@
                        운영 - server=10.51.241.245,7702;database=LHS;uid=lces_select;pwd=select  

         광고 DID     : 개발 - server=10.51.247.1;database=DID;uid=sa;pwd=Admin!@#$%
                        운영 - server=10.119.1.22;database=DID;uid=lc_dsi;pwd=info0420!
         */
    }
}