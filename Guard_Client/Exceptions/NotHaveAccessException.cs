using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Exceptions
{
    public class NotHaveAccessException : Exception
    {

        public string UserName { get; set; }
        public NotHaveAccessException(string userName)
        {
            UserName = userName;
        }

        public NotHaveAccessException(string message, string userName) : base(message)
        {
            UserName = userName;
        }

        public NotHaveAccessException(string message, Exception innerException, string userName) : base(message, innerException)
        {
            UserName = userName;
        }

        protected NotHaveAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
