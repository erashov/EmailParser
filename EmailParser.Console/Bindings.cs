using EmailParser.Service;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailParser.Console
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IEmailService>().To<EmailService>();
            Bind<ISoapService>().To<SoapService>();
        }
    }
}
