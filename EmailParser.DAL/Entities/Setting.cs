using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace EmailParser.DAL.Entities
{
   // [Serializable]
    public class Setting
    {
        public string Name
        {
            get;
            set;
        }

        public string InputMail
        {
            get;
            set;
        }

        public string InputMailPassword
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string ServiceUrl
        {
            get;
            set;
        }

        public string ImapServer
        {
            get;
            set;
        }

        public short ImapPort
        {
            get;
            set;
        }

        public string SmptServer
        {
            get;
            set;
        }

        public short? SmptPort
        {
            get;
            set;
        }

        public string OutputMail
        {
            get;
            set;
        }

        public string ProcessName
        {
            get;
            set;
        }

        public string RegexMask
        {
            get;
            set;
        }

        public ParamSetting[] ParamSettings
        {
            get;
            set;
        }


    }
}
