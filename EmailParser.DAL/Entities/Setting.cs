using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace EmailParser.DAL.Entities
{
    [Serializable]
    public class Setting
    {
    
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("InputMail")]
        public string InputMail { get; set; }
        [XmlElement("InputMailPassword")]
        public string InputMailPassword { get; set; }
        [XmlElement("Subject")]
        public string Subject { get; set; }
        [XmlElement("ServiceUrl")]
        public string ServiceUrl { get; set; }
        [XmlElement("ImapServer")]
        public string ImapServer { get; set; }
        [XmlElement("ImapPort")]
        public short ImapPort { get; set; }
        [XmlElement("ProcessName")]
        public string ProcessName { get; set; }
        [XmlElement("RegexMask")]
        public string RegexMask { get; set; }
        [XmlArray("ParamSettings")]
        [XmlArrayItem("ParamSetting")]
        public ParamSetting[] ParamSettings { get; set; }

    }
}
