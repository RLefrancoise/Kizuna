using System;
using ChatServerExample.Packets.PacketData;
using ChatServerExample.Packets.Server;
using Kizuna;
using Kizuna.Json;

namespace ChatServerExample.Packets.Client
{
    public class ClientChatMessage : IncomingJsonPacket<ChatMessage>
    {
        public ClientChatMessage(IncomingPacketInfo info) : base(info) {}

        public override void HandlePacket(object data = null)
        {
            ChatServer server = (ChatServer) data;

            Console.WriteLine($"{PacketMessage.Author} wrote {PacketMessage.Message}");
            server.BroadcastPacket(new ServerMessageReceived(PacketMessage));
        }
    }
}
