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
            //var kernel = new StandardKernel();
            //kernel.Load(Assembly.GetExecutingAssembly());
            //var details = kernel.Get<IEmailService>();
            //var soap = kernel.Get<ISoapService>();
            //List<Setting> result = new List<Setting>();


           // string path = System.Environment.CurrentDirectory + @"\settings.xml";
            //CarCollection cars = null;
            //string path = "cars.xml";

            // XmlSerializer serializer = new XmlSerializer(typeof(SettingList));

            string xmlString = "<Products><Product><Id>1</Id><Name>My XML product</Name></Product><Product><Id>2</Id><Name>My second product</Name></Product></Products>";
            XmlSerializer serializer = new XmlSerializer(typeof(List<Product>), new XmlRootAttribute("Products"));
            StringReader stringReader = new StringReader(xmlString);
            List<Product> productList = (List<Product>)serializer.Deserialize(stringReader);
           
            //XmlSerializer serializer = new XmlSerializer(typeof(List<Setting>), new XmlRootAttribute("Settings"));

            //StreamReader reader = new StreamReader(path);

            //result = (List<Setting>)serializer.Deserialize(reader);
            //reader.Close();

            System.Console.ReadKey();
        }


    }
}
