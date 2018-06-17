using System.Text;
using Newtonsoft.Json;

namespace Kizuna.Json
{
    /// <inheritdoc cref="AbstractPacket"/>
    /// <summary>
    /// Base class for any JSON packet
    /// </summary>
    /// <typeparam name="TPacketData">Type that describes the JSON data</typeparam>
    public abstract class AbstractJsonPacket<TPacketData> : AbstractPacket where TPacketData : new()
    {
        /// <summary>
        /// Data of the packet as a class describing the JSON data
        /// </summary>
        public TPacketData PacketMessage { get; protected set; }

        /// <summary>
        /// Construct a JSON packet from incoming packet data
        /// </summary>
        /// <param name="info">Incoming data</param>
        protected AbstractJsonPacket(IncomingPacketInfo info) : base(info)
        {
            PacketMessage = JsonConvert.DeserializeObject<TPacketData>(Encoding.UTF8.GetString(info.PacketData));
        }

        /// <summary>
        /// Construct a JSON packet by identifier and data as an object describing the JSON data
        /// </summary>
        /// <param name="identifier">identifier of the packet</param>
        /// <param name="message">Object describing the JSON data</param>
        protected AbstractJsonPacket(int identifier, TPacketData message) : base(identifier)
        {
            PacketMessage = message;
            PacketData = MessageToBytes();
        }

        protected override byte[] MessageToBytes()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PacketMessage));
        }
    }
}
