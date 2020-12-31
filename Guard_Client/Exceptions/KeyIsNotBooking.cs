using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Exceptions
{
    public class KeyIsNotBooking : Exception
    {

        public string Message { get; set; }

        public KeyIsNotBooking(string message)
        {
            Message = message;
        }


        public KeyIsNotBooking(string message, Exception innerException) : base(message, innerException)
        {
            Message = message;
        }


    }
}
