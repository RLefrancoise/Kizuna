using System;
using System.Net;
using ChatProtocol.PacketData;
using Kizuna;
using Kizuna.Json;

namespace ChatClientExample.Packets.Server
{
    public class ServerMessageReceived : IncomingJsonPacket<ChatMessage>
    {
        public ServerMessageReceived(IncomingPacketInfo info) : base(info) { }

        public override void HandlePacket(object data = null)
        {
            ChatClient client = (ChatClient) data;

            //Don't log if message comes from self
            //if ((Source.RemoteEndPoint as IPEndPoint).Equals(client.ClientSocket.RemoteEndPoint as IPEndPoint)) return;

            client.WriteToChat(PacketMessage.Author, PacketMessage.Message);
        }
    }
}
