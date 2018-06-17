using System.Net.Sockets;

namespace Kizuna.Xml
{
    /// <inheritdoc cref="AbstractXmlPacket{TPacketData}"/>
    /// <summary>
    /// Base class for any incoming XML packet.
    /// </summary>
    /// <typeparam name="TPacketData">Type of the object that describes the XML data</typeparam>
    public abstract class IncomingXmlPacket<TPacketData> : AbstractXmlPacket<TPacketData>, IIncomingPacket where TPacketData : new()
    {
        public Socket Source { get; }

        /// <summary>
        /// Construct a new incoming XML packet from incoming data
        /// </summary>
        /// <param name="info">Incoming data</param>
        protected IncomingXmlPacket(IncomingPacketInfo info) : base(info)
        {
            Source = info.Source;
        }

        public abstract void HandlePacket(object data = null);
    }
}
