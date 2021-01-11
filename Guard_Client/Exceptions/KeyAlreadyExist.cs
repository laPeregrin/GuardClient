using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Exceptions
{
    public class KeyAlreadyExist  : Exception
    {
        public KeyAlreadyExist(string aUDITORY_NumberKey)
        {
            KeyNumber = aUDITORY_NumberKey;
        }

        public KeyAlreadyExist(string message, string aUDITORY_NumberKey) : base(message)
        {
            KeyNumber = aUDITORY_NumberKey;
        }

        public KeyAlreadyExist(string message, Exception innerException, string aUDITORY_NumberKey) : base(message, innerException)
        {
            KeyNumber = aUDITORY_NumberKey;
        }

        protected KeyAlreadyExist(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string KeyNumber { get; set; }
    }
}
