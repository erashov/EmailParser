using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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
            if (!string.IsNullOrEmpty(setting.RegexMask))
            {
                list.AddRange(new List<ParamMessage> {
                new ParamMessage { Name = "mail_text", Value = text_body },
                new ParamMessage { Name = "date_mail", Value = date } });
            }
            return SOAPManual(setting, list);
        }

        private String SOAPManual(Setting setting, List<ParamMessage> list)
        {


            XmlDocument soapEnvelopeXml =(!string.IsNullOrEmpty(setting.RegexMask))? CreateSoapEnvelope(setting, list): CreateSoapEnvelope2(setting, list);
            HttpWebRequest webRequest = CreateWebRequest(setting.ServiceUrl);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                result = (((HttpWebResponse)response).StatusCode).ToString();
                if (string.IsNullOrEmpty(setting.RegexMask))
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        XDocument mydoc = XDocument.Load(rd);

                        XNamespace ns = mydoc.Root.GetDefaultNamespace();
                        IEnumerable<XElement> responses = mydoc.Descendants(ns + "startPN_ForpostResponse");
                        foreach (XElement respons in responses)
                        {
                          var  strResponseCode = (string)respons.Element(ns + "startPN_ForpostResult");
                            if (strResponseCode != "1") { result = "Bad Request"; }


                        }
                        //var pricres = from o in mydoc.Root.Elements(ns + "response").Elements(ns + "response")
                        //             select (int)o.Element(ns + "messageData");


                    }
                }

            }

            return result;
        }

        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
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
        private static XmlDocument CreateSoapEnvelope2(Setting setting, List<ParamMessage> list)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();

            string param = string.Empty;
            foreach (var item in list)
            {
                if (!string.IsNullOrWhiteSpace(item.Value))
                {
                    param += $@"<{item.Name}>{item.Value}</{item.Name}>\r\n";
                }
            }

            string xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                          <soap12:Body>
                            <startPN_Forpost  xmlns=""http://tempuri.org/"">
                                {param}                          
                            </startPN_Forpost>
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
