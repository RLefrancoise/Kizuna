using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Kizuna.Xml
{
    /// <inheritdoc cref="AbstractPacket"/>
    /// <summary>
    /// Base class for any XML packet
    /// </summary>
    /// <typeparam name="TPacketData">Type that describes the XML data</typeparam>
    public abstract class AbstractXmlPacket<TPacketData> : AbstractPacket where TPacketData : new()
    {
        /// <summary>
        /// Serializer to convert XML to packet message and vice versa
        /// </summary>
        protected XmlSerializer Serializer { get; }
        /// <summary>
        /// Data of the packet as a class describing the XML data
        /// </summary>
        public TPacketData PacketMessage { get; protected set; }

        /// <summary>
        /// Construct a XML packet by identifier and data as an object describing the XML data
        /// </summary>
        /// <param name="identifier">identifier of the packet</param>
        /// <param name="message">Object describing the JSON data</param>
        protected AbstractXmlPacket(int identifier, TPacketData message) : base(identifier)
        {
            Serializer = new XmlSerializer(typeof(TPacketData));
            PacketMessage = message;
            PacketData = MessageToBytes();
        }

        /// <summary>
        /// Construct a XML packet from incoming packet data
        /// </summary>
        /// <param name="info">Incoming data</param>
        protected AbstractXmlPacket(IncomingPacketInfo info) : base(info)
        {
            Serializer = new XmlSerializer(typeof(TPacketData));
            PacketMessage = (TPacketData) Serializer.Deserialize(new StringReader(Encoding.UTF8.GetString(info.PacketData)));
        }

        protected override byte[] MessageToBytes()
        {
            StringWriter sw = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sw);
            Serializer.Serialize(writer, PacketMessage);
            return Encoding.UTF8.GetBytes(sw.ToString());
        }
    }
}
