using ChatProtocol;
using ChatProtocol.PacketData;
using Kizuna.Json;

namespace ChatClientExample.Packets.Client
{
    /// <inheritdoc cref="AbstractJsonPacket{ChatMessage}"/>
    /// <summary>
    /// Packet used to send a new chat message to the server
    /// </summary>
    public class ClientSendMessage : AbstractJsonPacket<ChatMessage>
    {
        /// <summary>
        /// Constructs a new ClientSendMessage packet
        /// </summary>
        /// <param name="message">message to send</param>
        public ClientSendMessage(ChatMessage message) : base(ClientPackets.ChatMessage, message)
        {
        }
    }
}
