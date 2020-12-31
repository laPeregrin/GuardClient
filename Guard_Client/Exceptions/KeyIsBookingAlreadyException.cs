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
            AUDITORY_NumberKey = aUDITORY_NumberKey;
        }

        public KeyIsBookingAlreadyException(string aUDITORY_NumberKey , string message) : base(message)
        {
            AUDITORY_NumberKey = aUDITORY_NumberKey;
        }

        public KeyIsBookingAlreadyException(string aUDITORY_NumberKey, string message, Exception innerException) : base(message, innerException)
        {
            AUDITORY_NumberKey = aUDITORY_NumberKey;
        }

        protected KeyIsBookingAlreadyException(string aUDITORY_NumberKey, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            AUDITORY_NumberKey = aUDITORY_NumberKey;
        }

        string AUDITORY_NumberKey { get; set; }
    }
}
