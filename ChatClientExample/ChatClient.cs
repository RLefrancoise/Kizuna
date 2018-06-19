using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatClientExample.Packets.Client;
using ChatProtocol.PacketData;
using Kizuna;

namespace ChatClientExample
{
    /// <summary>
    /// Chat client that connects to a chat server and allows users to chat by messages.
    /// </summary>
    public class ChatClient
    {
        /// <summary>
        /// Current user input. Should not be accessed directly as it is not thread-safe
        /// </summary>
        private string _currentInput;

        /// <summary>
        /// Current user input with lock management to access it from any thread
        /// </summary>
        private string CurrentInput
        {
            get
            {
                lock (_lock)
                {
                    return _currentInput;
                }
            }

            set
            {
                lock (_lock)
                {
                    _currentInput = value;
                }
            }
        }

        /// <summary>
        /// Nick name of the user
        /// </summary>
        public string NickName { get; private set; }

        /// <summary>
        /// Builder of server packets
        /// </summary>
        private readonly ServerPacketBuilder _packetBuilder;

        /// <summary>
        /// Lock to access input of user in a thread-safe way
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// To synchronize connection
        /// </summary>
        private readonly ManualResetEvent _connectDone = new ManualResetEvent(false);

        /// <summary>
        /// Socket of the client
        /// </summary>
        public Socket ClientSocket { get; }

        /// <summary>
        /// To abort all the threads when exiting the application
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Called when connection is lost
        /// </summary>
        public Action OnConnectionLost;

        /// <summary>
        /// Construct a new chat client
        /// </summary>
        /// <param name="address">Address of the server</param>
        /// <param name="port">Port number of the server</param>
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

        /// <summary>
        /// Start the chat client
        /// </summary>
        public void Start()
        {
            Task.Factory.StartNew(ListenToServer, _cancellationTokenSource.Token);

            DisplayChatWelcome();

            //Ask Nickname
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Choose a nickname: ");
            Console.ResetColor();
            NickName = Console.ReadLine();

            ReadUserInput();
        }

        private static void DisplayChatWelcome()
        {
            LogInfo("-------------------------------------------");
            LogInfo("Welcome to this Kizuna Chat Client Example!");
            LogInfo("-------------------------------------------");
            LogInfo("Chat commands:");
            Console.WriteLine();
            LogInfo("/exit - Exit the chat");
            LogInfo("-------------------------------------------");
            Console.WriteLine();
        }

        /// <summary>
        /// Is input a command ?
        /// </summary>
        /// <param name="input">input to check</param>
        /// <returns>true if input is a command, false otherwise</returns>
        private static bool InputIsCommand(string input)
        {
            return input.StartsWith("/");
        }

        /// <summary>
        /// Handle chat command
        /// </summary>
        /// <param name="command">command to handle</param>
        /// <returns>true to continue to read user input after command execution, false otherwise</returns>
        private static bool HandleChatCommand(string command)
        {
            if (!command.StartsWith("/")) return true;

            switch (command)
            {
                case "/exit":
                    return false;
                default:
                    LogError("Unknown command {0}", command);
                    return true;
            }
        }

        /// <summary>
        /// Read keyboard of user
        /// </summary>
        private void ReadUserInput()
        {
            while (true)
            {
                //Refresh input (here we should have nickname with empty input as it was reset at the end of the loop)
                RefreshUserInput();

                //Listen user keys
                ConsoleKeyInfo keyInfo;
                CurrentInput = "";
                do
                {
                    keyInfo = Console.ReadKey();
                    
                    //If backspace, remove last character
                    if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (CurrentInput.Length > 0)
                        {
                            CurrentInput = CurrentInput.Remove(CurrentInput.Length - 1);
                            RefreshUserInput();
                        }
                        else
                        {
                            //if backspace and no user input, prevent cursor to be before beginning of input
                            // + 2 are for : and space character after nickname
                            Console.SetCursorPosition(NickName.Length + 2, Console.CursorTop);
                        }
                        
                        continue;
                    }

                    //Don't allow control + any character
                    if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0) continue;
                    if (char.IsControl(keyInfo.KeyChar)) continue;

                    //If different from enter, add it to current input
                    if (keyInfo.Key != ConsoleKey.Enter) CurrentInput += keyInfo.KeyChar;
                }
                while (keyInfo.Key != ConsoleKey.Enter);

                //Clear current line (typed message will be displayed once the server got it and send it back to the client)
                ClearCurrentConsoleLine();

                //Handle chat command & exit if command stops the user input to be read
                if (InputIsCommand(CurrentInput))
                {
                    if (!HandleChatCommand(CurrentInput)) break;

                    //Clear current input after command handling
                    CurrentInput = "";
                    continue;
                }

                //Don't send empty messages
                if (CurrentInput.Length == 0) continue;

                //Send message to the server
                ClientSocket.SendPacket(new ClientSendMessage(
                    new ChatMessage
                    {
                        Author = NickName,
                        Message = CurrentInput
                    }));

                //Reset current input
                CurrentInput = "";
            }
        }

        /// <summary>
        /// Clear current line
        /// </summary>
        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        /// <summary>
        /// Add a message to the chat
        /// </summary>
        /// <param name="author"></param>
        /// <param name="message"></param>
        public void WriteToChat(string author, string message)
        {
            //First clear current line
            ClearCurrentConsoleLine();

            //Write to chat
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{author}: ");
            Console.ResetColor();
            Console.WriteLine(message);

            RefreshUserInput();
        }

        /// <summary>
        /// Refresh user current input
        /// </summary>
        private void RefreshUserInput()
        {
            //First clear current line
            ClearCurrentConsoleLine();

            //Redisplay current user input
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{NickName}: ");
            Console.ResetColor();
            Console.Write(CurrentInput);
        }

        /// <summary>
        /// Listen to the server
        /// </summary>
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

        /// <summary>
        /// Callback called when connecting to the server
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Complete the connection.  
                ClientSocket.EndConnect(ar);

                // Signal that the connection has been made.  
                _connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Log info in the chat
        /// </summary>
        /// <param name="message">Info message</param>
        /// <param name="parameters">Format paramters</param>
        public static void LogInfo(string message, params object[] parameters)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message, parameters);
            Console.ResetColor();
        }

        /// <summary>
        /// Log error in the chat
        /// </summary>
        /// <param name="error">Error message</param>
        /// <param name="parameters">Format parameters</param>
        public static void LogError(string error, params object[] parameters)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error, parameters);
            Console.ResetColor();
        }
    }
}
