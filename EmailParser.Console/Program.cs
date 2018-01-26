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
            var details = kernel.Get<IEmailService>();
            var soap = kernel.Get<ISoapService>();
            SettingList result = new SettingList();

        
            string data;
            string path = System.Environment.CurrentDirectory + @"\settings.xml";
            //CarCollection cars = null;
            //string path = "cars.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(SettingList));

            StreamReader reader = new StreamReader(path);
            result = (SettingList)serializer.Deserialize(reader);
            reader.Close();
            //Setting setting = new Setting();
         //   XmlSerializer serializer = new XmlSerializer(typeof(SettingList));
            ////  TextWriter textWriter = new StreamWriter(Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "settings.xml"));
            //string path = System.Environment.CurrentDirectory + @"\settings.xml";
   

            //Setting setting = new Setting();
            //  details.PaerserEmailAsync(setting, soap);
            //var employee = new Employee(details);
            //string result = employee.Getdetails();

            // System.Console.WriteLine(result);
            System.Console.ReadKey();
        }
    }
}
