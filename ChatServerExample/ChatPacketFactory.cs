using ChatServerExample.Packets.Client;
using Kizuna;

namespace ChatServerExample
{
    public class ChatPacketFactory : IPacketFactory
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
