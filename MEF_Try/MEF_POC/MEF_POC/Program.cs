using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;


namespace MEF_POC
{
    public class Program
    {
        [Import()]
        public string PluggableString { get; set; }
        //[Import(typeof(IModule))] 
        //public IModule _module;
        static void Main(string[] args)
        {
            
            Program p  = new Program();
            p.Compose();

           // var connectors = Directory.GetDirectories(plugName);
           
        }

        public void Compose()   
        {
            var files = ConfigurationManager.AppSettings.Get("pathForDlls");
            //var dlls = Directory.GetFiles(files);
            //AssemblyCatalog catalog = new AssemblyCatalog(dlls.First());

            ////dlls.ToList().ForEach(connect => catalog.Catalogs.Add(new DirectoryCatalog(connect)));
            //var container = new CompositionContainer(catalog);
            //container.ComposeParts(this);
            //Console.WriteLine(PluggableString);
            //Console.ReadLine();


            var dlls = Directory.GetFiles(files);
            AssemblyCatalog catalog = new AssemblyCatalog(Assembly.LoadFrom(dlls.First()));
            //var type = catalog.Assembly.GetType(typeof(IModule).ToString());
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            Console.WriteLine(PluggableString);
            Console.ReadLine();
        }
    }
}
