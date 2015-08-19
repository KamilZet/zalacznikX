using InputWindow;
using System;
using System.Security;
using System.Configuration;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;
using System.Linq;

namespace zalacznikX
{
    static class Program
    {
        static void Main(string[] args)
        {
#if (!DEBUG)

        if (Environment.UserInteractive)
        {
            string parameter = string.Concat(args);
            string test = "test"  ;
            switch (parameter)
            {
                case "--install":
                    if (!ServiceController.GetServices().Any(s => s.ServiceName == "zalacznikX"))
                        ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                    break;
                case "--uninstall":
                    ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
                    break;
            }
        }
        else
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Sluchaj() 
            };
            ServiceBase.Run(ServicesToRun);
        }
#else

            Sluchaj s = new Sluchaj();
            s.DebugStart();
           
#endif  
        }
    }
}
