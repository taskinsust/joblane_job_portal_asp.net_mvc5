using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Joblanes.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException()
            : base("Message Data")
        {

        }

        public MessageException(string message)
            : base(message)
        {

        }

    }
}
