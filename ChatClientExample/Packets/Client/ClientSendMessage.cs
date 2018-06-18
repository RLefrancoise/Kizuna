using ChatProtocol;
using ChatProtocol.PacketData;
using Kizuna.Json;

namespace ChatClientExample.Packets.Client
{
    public class ClientSendMessage : AbstractJsonPacket<ChatMessage>
    {
        public ClientSendMessage(ChatMessage message) : base(ClientPackets.ChatMessage, message)
        {
        }
    }
}
