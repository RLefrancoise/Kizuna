using System.Net.Sockets;
using Google.Protobuf;

namespace Kizuna.Protobuf
{
    /// <inheritdoc cref="AbstractProtobufPacket{TPacketData}"/>
    /// <summary>
    /// Base class for any incoming Protobuf packet.
    /// </summary>
    /// <typeparam name="TPacketData">Type of the object that describes the Protobuf data</typeparam>
    public abstract class IncomingProtobufPacket<TPacketData> : AbstractProtobufPacket<TPacketData>, IIncomingPacket where TPacketData : IMessage<TPacketData>, new()
    {
        public Socket Source { get; }

        /// <summary>
        /// Construct a new incoming Protobuf packet from incoming data
        /// </summary>
        /// <param name="info">Incoming data</param>
        protected IncomingProtobufPacket(IncomingPacketInfo info) : base(info)
        {
            Source = info.Source;
        }

        public abstract void HandlePacket(object data = null);
    }
}