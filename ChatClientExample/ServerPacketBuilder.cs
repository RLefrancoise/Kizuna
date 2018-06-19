using ChatProtocol;
using Kizuna;

namespace ChatClientExample
{
    /// <inheritdoc cref="AbstractPacketBuilder"/>
    /// <summary>
    /// Packet builder for server packets
    /// </summary>
    public class ServerPacketBuilder : AbstractPacketBuilder
    {
        /// <summary>
        /// Constructs a new server packet builder
        /// </summary>
        public ServerPacketBuilder()
        {
            RegisterPacketIdentifier(PacketTypes.Chat, ServerPackets.MessageReceived);

            RegisterFactory(PacketTypes.Chat, new ServerChatPacketFactory());
        }
    }
}
