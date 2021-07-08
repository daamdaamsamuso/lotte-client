using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LotteCinemaService.Model.CMS
{
    [DataContract]
    public struct stAdvertisement
    {
        [DataMember]
        public int seq;
        [DataMember]
        public string AdverMainInfoCode;
        [DataMember]
        public string ClientSeq;
        [DataMember]
        public string CampaignName;
        [DataMember]
        public string CinemaCode;
        [DataMember]
        public string CinemaName;
        [DataMember]
        public string StartDate;
        [DataMember]
        public string EndDate;
        [DataMember]
        public string AccountCode;
        [DataMember]
        public string SalesPart;
        [DataMember]
        public string AdverType;
    }

    [DataContract]
    public struct stMultiCubeAD
    {
        [DataMember]
        public string key;
        [DataMember]
        public int ClientSeq;
        [DataMember]
        public string CampaignName;
        [DataMember]
        public string CinemaCode;
        [DataMember]
        public string CinemaName;
        [DataMember]
        public string StartDate;
        [DataMember]
        public string EndDate;
        [DataMember]
        public int AccountSeq;
        [DataMember]
        public string SalesPart;
        [DataMember]
        public string Extension;
        [DataMember]
        public TimeSpan Duration;
    }

    [DataContract]
    public struct stScheduleAD
    {
        [DataMember]
       public string startTime;
        [DataMember]
        public string endTime;
    }
}
