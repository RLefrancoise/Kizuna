using ChatProtocol;
using Kizuna;

namespace ChatClientExample
{
    public class ServerPacketBuilder : AbstractPacketBuilder
    {
        public ServerPacketBuilder()
        {
            RegisterPacketIdentifier(PacketTypes.Chat, ServerPackets.MessageReceived);

            RegisterFactory(PacketTypes.Chat, new ServerChatPacketFactory());
        }
    }
}
