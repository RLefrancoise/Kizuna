using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Kizuna;

namespace ChatServerExample
{
    public class ChatServer
    {
        private readonly ClientPacketBuilder _packetBuilder;

        private readonly List<Socket> _clients;

        private readonly object _lock = new object();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public ChatServer()
        {
            _packetBuilder = new ClientPacketBuilder();
            _clients = new List<Socket>();
        }

        public void Start()
        {
            AsynchronousSocketListener.OnConnectionOpened += NewClientConnected;

            Task.Factory.StartNew(CheckClientTimeout, _cancellationTokenSource.Token);
            Task.Factory.StartNew(ListenClients, _cancellationTokenSource.Token);

            AsynchronousSocketListener.StartListening();

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void BroadcastPacket(IPacket packet)
        {
            lock (_lock)
            {
                _clients.BroadcastPacket(packet);
            }
        }

        private void NewClientConnected(Socket client)
        {
            lock (_lock)
            {
                Console.WriteLine($"Client {client.RemoteEndPoint as IPEndPoint} connected");
                _clients.Add(client);
                //AbstractPacketBuilder.ReadAndHandlePacketFromSocket(client, _packetBuilder, this);
            }
        }

        private void ListenClients()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                lock (_lock)
                {
                    foreach (var client in _clients)
                    {
                        if (client.Available == 0) continue;
                        
                        AbstractPacketBuilder.ReadAndHandlePacketFromSocket(client, _packetBuilder, this);
                    }
                }
            } 
        }

        private async void CheckClientTimeout()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                lock (_lock)
                {
                    for(var i = 0 ; i < _clients.Count ; i++)
                    {
                        if (!_clients[i].Poll(10, SelectMode.SelectRead) || _clients[i].Available != 0) continue;

                        Console.WriteLine($"Client {_clients[i].RemoteEndPoint as IPEndPoint} disconnected");
                        _clients.Remove(_clients[i]);
                        i--;
                    }
                }

                //Check every 2 seconds
                await Task.Delay(2000);
            }
        }
    }
}
