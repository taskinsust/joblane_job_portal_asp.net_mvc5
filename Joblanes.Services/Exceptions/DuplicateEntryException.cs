using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Joblanes.Exceptions
{
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException()
            : base("Duplicate Found")
        {

        }

        public DuplicateEntryException(string message)
            : base(message)
        {

        }
    }
}
