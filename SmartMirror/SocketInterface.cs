using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror
{
    class SocketInterface
    {
        private static TcpListener _server;
        public static TcpListener start()
        {
            if (_server == null)
            {
                _server = new TcpListener(new System.Net.IPEndPoint(IPAddress.Any,54321));
            }
            _server.Start();
            return _server;
        }
        public static byte[] encode(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }
        public static string decode(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}
