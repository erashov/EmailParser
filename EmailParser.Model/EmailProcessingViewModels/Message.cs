using System;
using System.Collections.Generic;
using System.Text;

namespace EmailParser.Model.EmailParserViewModels
{

    public class Message
    {
      
        public string MessageID { get; set; }
        public string Subject { get; set; }
        public List<ParamMessage> Params { get; set; }
    }
}
