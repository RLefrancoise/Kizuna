using ChatProtocol;
using ChatServerExample.Packets.Client;
using Kizuna;

namespace ChatServerExample
{
    public class ClientChatPacketFactory : IPacketFactory
    {
        public IIncomingPacket FromPacketInfo(IncomingPacketInfo info)
        {
            switch (info.Identifier)
            {
                case ClientPackets.ChatMessage:
                    return new ClientChatMessage(info);
            }

            return null;
        }
    }
}
