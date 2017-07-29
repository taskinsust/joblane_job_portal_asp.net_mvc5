using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joblanes.Services.Test.ObjectFactory
{
    public interface IObjectFactoryBase
    {
        void DeleteAll();
        string GetPcUserName();
        int Level { get; set; }
        string Name { get; set; }
        int ListStartIndex { get; set; }

    }
}
