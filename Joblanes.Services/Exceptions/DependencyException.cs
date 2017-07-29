using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Joblanes.Exceptions
{
    public class DependencyException : Exception
    {

        public DependencyException()
            : base("Dependency Found")
        {

        }

        public DependencyException(string message)
            : base(message)
        {

        }
    }
}
