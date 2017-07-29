using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Joblanes.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException()
            : base("Not Found")
        {

        }

        public DataNotFoundException(string message)
            : base(message)
        {

        }

    }
}
