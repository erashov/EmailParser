using EmailParser.DAL.Entities;
using EmailParser.Model.EmailParserViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EmailParser.Service
{
    public interface ISoapService
    {
        HttpWebRequest SendRequest(string message);
        string SendRequest(Setting setting, List<ParamMessage> list, string text_body, string date);
    }
}
