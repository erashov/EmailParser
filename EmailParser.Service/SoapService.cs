using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using EmailParser.DAL.Entities;
using EmailParser.Model.EmailParserViewModels;

namespace EmailParser.Service
{
    public class SoapService : ISoapService
    {
        public HttpWebRequest SendRequest(string message)
        {
            throw new NotImplementedException();
        }

        public string SendRequest(Setting setting, List<ParamMessage> list, string text_body, string date)
        {
            list.AddRange(new List<ParamMessage> {
                new ParamMessage { Name = "mail_text", Value = text_body },
                new ParamMessage { Name = "date_mail", Value = date } });
            return SOAPManual(setting, list);
        }

        private String SOAPManual(Setting setting, List<ParamMessage> list)
        {

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(setting, list);
            HttpWebRequest webRequest = CreateWebRequest(setting.ServiceUrl);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                result = (((HttpWebResponse)response).StatusCode).ToString();

            }

            return result;
        }

        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            // webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(Setting setting, List<ParamMessage> list)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();

            string param = string.Empty;
            foreach (var item in list)
            {
                if (!string.IsNullOrWhiteSpace(item.Value))
                {
                    param += $@"<ArrayOfString>\r\n
                             <string>{item.Name}</string>\r\n
                             <string>{item.Value}</string>\r\n
                             </ArrayOfString>\r\n";
                }
            }

            string xml =$@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                          <soap12:Body>
                            <LaunchProcessUltimus xmlns=""http://tempuri.org/"">
                              <process_name>{setting.ProcessName}</process_name>
                              <var>
                                {param}
                              </var>
                            </LaunchProcessUltimus>
                          </soap12:Body>
                        </soap12:Envelope>";
            soapEnvelopeXml.LoadXml(xml);

            return soapEnvelopeXml;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
