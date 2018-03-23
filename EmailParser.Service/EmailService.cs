using EmailParser.DAL.Entities;
using EmailParser.Model.EmailParserViewModels;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EmailParser.Service
{
    public class EmailService : IEmailService
    {
        public void PaerserEmailAsync(Setting setting, ISoapService soapService)
        {
            List<Message> messages = new List<Message>();

            using (var client = new ImapClient())
            {
              //  client.SslProtocols= System.Security.Authentication.SslProtocols.Ssl3;
                client.Connect(setting.ImapServer, setting.ImapPort, SecureSocketOptions.Auto);

                client.Authenticate(setting.InputMail, setting.InputMailPassword);

                client.Inbox.Open(FolderAccess.ReadWrite);
              
                var uids = client.Inbox.Search(SearchQuery.New);
                uids = client.Inbox.Search(SearchQuery.NotSeen);
             
                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);

                    if (message.Subject == setting.Subject && !setting.SmptPort.HasValue)
                    {
                        var ItemRegex = new Regex(setting.RegexMask, RegexOptions.Compiled);
                        var AllParamList = ItemRegex.Matches(message.TextBody)
                                            .Cast<Match>()
                                            .Select(m => new
                                            {
                                                Name = m.Groups[1].ToString(),
                                                Value = m.Groups[2].ToString()
                                            })
                                            .ToList();
                        var paramList = AllParamList.Join(setting.ParamSettings, ap => ap.Name, cp => cp.FullName, (paramsetting, parammessage) => new ParamMessage { Name = parammessage.Name, Value = paramsetting.Value }).ToList();
                        var resultService = soapService.SendRequest(setting, paramList, message.TextBody, message.Date.Date.ToString());
                        if (resultService == "OK")
                        {
                            client.Inbox.AddFlags(uid, MessageFlags.Seen, true);

                        }
                    }
                    else if (message.Subject.Contains(setting.Subject) && this.SendEmailAsync(setting, setting.OutputMail, message.Subject, message.TextBody))
                    {
                        client.Inbox.AddFlags(uid, MessageFlags.Seen, true, default(CancellationToken));
                    }
                }
                client.Disconnect(true);
            }
           // return Task.CurrentId;
        }

        public void MarkAsRead(Setting setting, List<Message> messages)
        {
            using (var client = new ImapClient())
            {

                client.Connect(setting.ImapServer, setting.ImapPort, SecureSocketOptions.SslOnConnect);

                client.Authenticate(setting.InputMail, setting.InputMailPassword);

                client.Inbox.Open(FolderAccess.ReadWrite);
                var uids = client.Inbox.Search(SearchQuery.New);

                uids = client.Inbox.Search(SearchQuery.NotSeen);
                foreach (var uid in uids)
                {
                    if (client.Inbox.GetMessage(uid)?.MessageId == messages.FirstOrDefault().MessageID)
                    {
                        var ms = client.Inbox.GetMessage(uid);
                    }
                }
            }
          //  return Task.CompletedTask;
        }

        public bool SendEmailAsync(Setting setting, string email, string subject, string message)
        {
            bool result2 = true;
            try
            {
                MimeMessage mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(setting.InputMail));
                mimeMessage.To.Add(new MailboxAddress(email));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = message
                };
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(setting.ImapServer, setting.SmptPort.Value, SecureSocketOptions.Auto, default(CancellationToken));
                    client.Authenticate(setting.InputMail, setting.InputMailPassword, default(CancellationToken));
                    client.Send(mimeMessage, default(CancellationToken), null);
                    client.Disconnect(true, default(CancellationToken));
                    return result2;
                }
            }
            catch (Exception ex)
            {
                result2 = false;
                throw ex;
            }
        }
    }
}
