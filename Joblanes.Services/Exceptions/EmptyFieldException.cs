using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Joblanes.Exceptions
{
    public class EmptyFieldException : Exception
    {
        public EmptyFieldException()
            : base("Empty field found")
        {

        }

        public EmptyFieldException(string message)
            : base(message)
        {

        }
    }
}
