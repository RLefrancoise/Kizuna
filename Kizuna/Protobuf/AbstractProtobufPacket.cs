using Google.Protobuf;

namespace Kizuna.Protobuf
{
    /// <inheritdoc cref="AbstractPacket"/>
    /// <summary>
    /// Base class for any Protobuf packet
    /// </summary>
    /// <typeparam name="TPacketData">Type that describes the Protobuf data</typeparam>
    public abstract class AbstractProtobufPacket<TPacketData> : AbstractPacket where TPacketData : IMessage<TPacketData>, new()
    {
        /// <summary>
        /// Data of the packet as a class describing the Protobuf data
        /// </summary>
        public TPacketData PacketMessage { get; protected set; }

        /// <summary>
        /// Construct a Protobuf packet by identifier and data as an object describing the Protobuf data
        /// </summary>
        /// <param name="identifier">identifier of the packet</param>
        /// <param name="message">Object describing the JSON data</param>
        protected AbstractProtobufPacket(int identifier, TPacketData message) : base(identifier)
        {
            PacketMessage = message;
            PacketData = MessageToBytes();
        }

        /// <summary>
        /// Construct a Protobuf packet from incoming packet data
        /// </summary>
        /// <param name="info">Incoming data</param>
        protected AbstractProtobufPacket(IncomingPacketInfo info) : base(info)
        {
            TPacketData data = new TPacketData();
            PacketMessage = (TPacketData)data.Descriptor.Parser.ParseFrom(info.PacketData);
        }

        protected override byte[] MessageToBytes()
        {
            return PacketMessage.ToByteArray();
        }
    }
}
