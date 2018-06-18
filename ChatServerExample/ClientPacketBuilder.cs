using ChatProtocol;
using Kizuna;

namespace ChatServerExample
{
    public class ClientPacketBuilder : AbstractPacketBuilder
    {
        public ClientPacketBuilder()
        {
            RegisterPacketIdentifier(PacketTypes.Chat, ClientPackets.ChatMessage);

            RegisterFactory(PacketTypes.Chat, new ClientChatPacketFactory());
        }
    }
}
