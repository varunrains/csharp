using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEF_export
{
    
    public interface IModule
    {
        string ModuleName { get; }

        void start();

        void stop();
    }
}
