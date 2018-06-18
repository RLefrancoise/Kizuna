using ChatClientExample.Packets.Server;
using ChatProtocol;
using Kizuna;

namespace ChatClientExample
{
    public class ServerChatPacketFactory : IPacketFactory
    {
        public IIncomingPacket FromPacketInfo(IncomingPacketInfo info)
        {
            switch (info.Identifier)
            {
                case ServerPackets.MessageReceived:
                    return new ServerMessageReceived(info);
            }

            return null;
        }
    }
}
