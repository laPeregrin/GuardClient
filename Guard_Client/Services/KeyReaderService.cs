using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Services
{
    public class KeyReaderService
    {
        public SerialPort serialPort {get;}
        public KeyReaderService()
        {
            serialPort = new SerialPort()
            {
                PortName = "COM1",
                BaudRate = 115200,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 7,
                Handshake = Handshake.None,
                Encoding = ASCIIEncoding.ASCII
            };
        }
    }
}
