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
        Task SendEmailAsync(string email, string subject, string message);
        Task PaerserEmailAsync(Setting setting, ISoapService soapService);
        Task MarkAsRead(Setting setting, List<Message> messages);
    }
}
