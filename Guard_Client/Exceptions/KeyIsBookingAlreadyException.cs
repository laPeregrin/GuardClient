using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Exceptions
{
    public class KeyIsBookingAlreadyException : Exception
    {
        public KeyIsBookingAlreadyException(string aUDITORY_NumberKey)
        {
            Username = aUDITORY_NumberKey;
        }

        public KeyIsBookingAlreadyException(string message, string aUDITORY_NumberKey) : base(message)
        {
            Username = aUDITORY_NumberKey;
        }

        public KeyIsBookingAlreadyException(string message, Exception innerException, string aUDITORY_NumberKey) : base(message, innerException)
        {
            Username = aUDITORY_NumberKey;
        }

        protected KeyIsBookingAlreadyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Username { get; set; }
    }
}
