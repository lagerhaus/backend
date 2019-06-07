using Lagerhaus.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagerhausServer.Errors
{
    public class InvalidMonthError:ValidationError
    {
        public InvalidMonthError(string message) : base(message)
        {
        }
    }
}
