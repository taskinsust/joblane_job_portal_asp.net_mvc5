using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Joblanes.Helper
{
    interface IConnectionBase
    {
         string MssqlConnectionString { get; set; }
    }
}
