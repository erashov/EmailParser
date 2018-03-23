using EmailParser.DAL.Entities;
using EmailParser.Service;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EmailParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var emailService = kernel.Get<IEmailService>();
            var soapService = kernel.Get<ISoapService>();
            string path = System.Environment.CurrentDirectory + @"\settings.xml"; 
            string readContents;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<Setting>), new XmlRootAttribute("Settings"));
            StringReader stringReader = new StringReader(readContents);
            List<Setting> settings = (List<Setting>)serializer.Deserialize(stringReader);
            foreach (var s in settings)
            {
                emailService.PaerserEmailAsync(s, soapService);

            }

        }



    }
}
