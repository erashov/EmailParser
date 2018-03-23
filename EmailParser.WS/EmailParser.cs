using EmailParser.DAL.Entities;
using EmailParser.Service;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EmailParser.WS
{
    public partial class EmailParser : ServiceBase
    {
        private System.Timers.Timer timer;
        public EmailParser()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Создаем таймер и выставляем его параметры
            this.timer = new System.Timers.Timer();
            this.timer.Enabled = true;
            //Интервал 10000мс - 10с.
            this.timer.Interval = 10000;
            this.timer.Elapsed +=
             new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.AutoReset = true;
            this.timer.Start();
        }

        protected override void OnStop()
        {
        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                var directoryPath = Path.GetDirectoryName(location);
                string path = directoryPath + @"\settings.xml";

                var kernel = new StandardKernel();
                kernel.Load(Assembly.GetExecutingAssembly());
                var emailService = kernel.Get<IEmailService>();
                var soapService = kernel.Get<ISoapService>();

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
            catch(Exception ex)
            {
                using (StreamWriter writetext = new StreamWriter(@"C:\logs\log.txt", true))
                {
                    writetext.WriteLine($"{DateTime.Now.ToString()}---{ex.Message}");
                }
            }
            




        }
    }
}
