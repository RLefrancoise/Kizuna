using ChatServerExample.Packets.PacketData;
using Kizuna.Json;

namespace ChatServerExample.Packets.Server
{
    public class ServerMessageReceived : AbstractJsonPacket<ChatMessage>
    {
        public ServerMessageReceived(ChatMessage message) : base(ServerPackets.MessageReceived, message)
        {
        }
    }
}
