using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace JobLanes.WcfService.DataContract
{
    [DataContract]
    public class RegionDataContract
    {
        private long m_Id;
        private string m_Name;
        private string m_status;
        [DataMember]
        public long Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [DataMember]
        public string Status
        {
            get { return m_status; }
            set { m_status = value; }
        }
    }
}