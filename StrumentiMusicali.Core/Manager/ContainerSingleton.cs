using Autofac;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StrumentiMusicali.Core.Manager
{
    public class IocContainerSingleton
    {
        static IContainer _container = null;

        public static IContainer GetContainer {
            get {
                if (_container == null)
                {

                    var container = new ContainerBuilder();
                    var allAssemblies = new List<Assembly>();
                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                    foreach (string dll in Directory.GetFiles(path, "Strumenti*.dll"))
                        allAssemblies.Add(Assembly.LoadFile(dll));
                    foreach (string dll in Directory.GetFiles(path, "Strumenti*.exe"))
                        allAssemblies.Add(Assembly.LoadFile(dll));

                    // var assemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(a=>a.FullName).ToList().ToArray();
                    container.RegisterAssemblyTypes(allAssemblies.ToArray()).Where(t => t.IsAssignableTo<IPlugInJob>())
                        .AsImplementedInterfaces();

                    _container= container.Build();
                }

                
                return _container;
            }
        }
    }
}
