using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Joblanes.Exceptions
{
    public class NullObjectException : Exception
    {
        public NullObjectException()
            : base("Null Value")
        {

        }

        public NullObjectException(string message)
            : base(message)
        {

        }
    }
}
