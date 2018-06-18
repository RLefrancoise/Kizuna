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
            client.WriteToChat(PacketMessage.Author, PacketMessage.Message);
        }
    }
}
