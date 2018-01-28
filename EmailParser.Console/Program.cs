using EmailParser.DAL.Entities;
using EmailParser.Service;
//using Ninject;
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
            //var kernel = new StandardKernel();
            //kernel.Load(Assembly.GetExecutingAssembly());
            //var details = kernel.Get<IEmailService>();
            //var soap = kernel.Get<ISoapService>();
            //List<Setting> result = new List<Setting>();


             string path = System.Environment.CurrentDirectory + @"\settings.xml";
            //CarCollection cars = null;
            //string path = "cars.xml";


            string readContents;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }


            XmlSerializer serializer = new XmlSerializer(typeof(List<Setting>), new XmlRootAttribute("Settings"));
            StringReader stringReader = new StringReader(readContents);
            List<Setting> productList = (List<Setting>)serializer.Deserialize(stringReader);


            //reader.Close();

            System.Console.ReadKey();
        }



    }
}
