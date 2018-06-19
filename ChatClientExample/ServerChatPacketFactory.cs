using ChatClientExample.Packets.Server;
using ChatProtocol;
using Kizuna;

namespace ChatClientExample
{
    /// <inheritdoc cref="IPacketFactory"/>
    /// <summary>
    /// Packet factory for server packets
    /// </summary>
    public class ServerChatPacketFactory : IPacketFactory
    {
        public IIncomingPacket FromPacketInfo(IncomingPacketInfo info)
        {
            switch (info.Identifier)
            {
                case ServerPackets.MessageReceived:
                    return new ServerMessageReceived(info);
                default:
                    return null;
            }
        }
    }
}
