using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEF_POC
{
    public interface IModule
    {
        string ModuleName { get; }

        void start();

        void stop();
    }
}
