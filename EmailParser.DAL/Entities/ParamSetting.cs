using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace EmailParser.DAL.Entities
{
    [XmlType("ParamSetting")]
    [Serializable]
    public class ParamSetting
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("FullName")]
        public string FullName { get; set; }


    }
}
