using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatClientExample.Packets.Client;
using ChatProtocol.PacketData;
using Kizuna;

namespace ChatClientExample
{
    public class ChatClient
    {
        public string NickName { get; private set; }

        private readonly ServerPacketBuilder _packetBuilder;

        private readonly ManualResetEvent _connectDone = new ManualResetEvent(false);
        public Socket ClientSocket { get; }

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Action OnConnectionLost;

        public ChatClient(string address, int port)
        {
            _packetBuilder = new ServerPacketBuilder();

            // Establish the remote endpoint for the socket.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(address);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEp = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            ClientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            ClientSocket.BeginConnect(remoteEp, ConnectCallback, ClientSocket);

            //Wait for a connection to the server
            _connectDone.WaitOne();
        }

        public void Start()
        {
            Task.Factory.StartNew(ListenToServer, _cancellationTokenSource.Token);

            Console.Write("Choose a nickname: ");
            NickName = Console.ReadLine();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{NickName}: ");
                Console.ResetColor();
                var input = Console.ReadLine();
                ClearCurrentConsoleLine();

                if (input == "/exit") break;

                ClientSocket.SendPacket(new ClientSendMessage(
                    new ChatMessage
                    {
                        Author = NickName,
                        Message = input
                    }));
            }
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public void WriteToChat(string author, string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{author}: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }

        private void ListenToServer()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (ClientSocket.Poll(10, SelectMode.SelectRead) && ClientSocket.Available == 0)
                {
                    OnConnectionLost?.Invoke();
                    break;
                }

                AbstractPacketBuilder.ReadAndHandlePacketFromSocket(ClientSocket, _packetBuilder, this);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Complete the connection.  
                ClientSocket.EndConnect(ar);

                Console.WriteLine("Client connected to {0}", ClientSocket.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                _connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
