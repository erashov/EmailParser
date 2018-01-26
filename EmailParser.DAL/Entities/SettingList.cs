using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EmailParser.DAL.Entities
{
    [Serializable()]
    [XmlType("Settings")]
   public class SettingList
    {
        [XmlArray("Settings")]
        [XmlArrayItem("Setting", typeof(Setting))]
        public Setting[] settings { get; set; }
    }
}
