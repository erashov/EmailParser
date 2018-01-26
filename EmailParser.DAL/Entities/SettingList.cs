using System;
using System.Xml.Serialization;

namespace EmailParser.DAL.Entities
{

    [XmlRootAttribute("Settings")]
   public class SettingList
    {
        [XmlArray("Settings")]
        [XmlArrayItem("Setting", typeof(Setting))]
        public Setting[] settings { get; set; }
    }
}
