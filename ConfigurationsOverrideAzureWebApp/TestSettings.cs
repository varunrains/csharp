using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationsOverrideAzureWebApp
{
    public class TestSettings
    {
        public Test1 Test1 { get; set; }
    }

    public class Test1
    {
        public string Test2 { get; set; }
    }
}
