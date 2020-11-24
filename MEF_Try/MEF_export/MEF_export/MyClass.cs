using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MEF_export
{
    //[Export(typeof(IModule))]
    public class MyClass : IModule
    {
        [Export]
        public string PluggableString
        {
            get
            {
                return "This is string from the class library";
            }
            set
            {
            }
        }
        public string ModuleName => "Data Package Module";

        public void start()
        {
            Console.WriteLine(ModuleName + " started");
        }

        public void stop()
        {
            Console.WriteLine(ModuleName + " stopped");
        }
    }
}
