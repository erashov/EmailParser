using EmailParser.DAL.Entities;
using EmailParser.Model.EmailParserViewModels;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailParser.Service
{
    public class EmailService : IEmailService
    {
        public Task PaerserEmailAsync(Setting setting, ISoapService soapService)
        {
            List<Message> messages = new List<Message>();

            using (var client = new ImapClient())
            {

                client.Connect(setting.ImapServer, setting.ImapPort, SecureSocketOptions.SslOnConnect);

                client.Authenticate(setting.InputMail, setting.InputMailPassword);

                client.Inbox.Open(FolderAccess.ReadWrite);
              
                var uids = client.Inbox.Search(SearchQuery.New);
                uids = client.Inbox.Search(SearchQuery.NotSeen);

                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);

                    if (message.Subject == setting.Subject)
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
                }
                client.Disconnect(true);
            }
            return Task.CompletedTask;
        }

        public Task MarkAsRead(Setting setting, List<Message> messages)
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
            return Task.CompletedTask;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
