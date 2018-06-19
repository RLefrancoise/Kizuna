using ChatProtocol.PacketData;
using Kizuna;
using Kizuna.Json;

namespace ChatClientExample.Packets.Server
{
    /// <inheritdoc cref="IncomingJsonPacket{ChatMessage}"/>
    /// <summary>
    /// Packet when a new chat message has been written in the chat
    /// </summary>
    public class ServerMessageReceived : IncomingJsonPacket<ChatMessage>
    {
        /// <summary>
        /// Constructs a new ServerMessageReceived packet
        /// </summary>
        /// <param name="info"></param>
        public ServerMessageReceived(IncomingPacketInfo info) : base(info) { }

        public override void HandlePacket(object data = null)
        {
            ChatClient client = (ChatClient) data;
            client.WriteToChat(PacketMessage.Author, PacketMessage.Message);
        }
    }
}
