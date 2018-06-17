using System.Net.Sockets;

namespace Kizuna.Json
{
    /// <inheritdoc cref="AbstractJsonPacket{TPacketData}"/>
    /// <summary>
    /// Base class for any incoming JSON packet.
    /// </summary>
    /// <typeparam name="TPacketData">Type of the object that describes the JSON data</typeparam>
    public abstract class IncomingJsonPacket<TPacketData> : AbstractJsonPacket<TPacketData>, IIncomingPacket where TPacketData : new()
    {
        public Socket Source { get; }

        /// <summary>
        /// Construct a new incoming JSON packet from incoming data
        /// </summary>
        /// <param name="info">Incoming data</param>
        protected IncomingJsonPacket(IncomingPacketInfo info) : base(info)
        {
            Source = info.Source;
        }

        public abstract void HandlePacket(object data = null);
    }
}
