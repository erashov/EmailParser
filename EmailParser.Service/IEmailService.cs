using EmailParser.DAL.Entities;
using EmailParser.Model.EmailParserViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailParser.Service
{
   public interface IEmailService
    {
        bool SendEmailAsync(Setting setting, string email, string subject, string message);

        void PaerserEmailAsync(Setting setting, ISoapService soapService);

        void MarkAsRead(Setting setting, List<Message> messages);
    }
}
