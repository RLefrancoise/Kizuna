using System.Net.Sockets;

namespace Kizuna
{
    /// <summary>
    /// Info of the packet.
    /// </summary>
    public sealed class IncomingPacketInfo
    {
        /// <summary>
        /// Identifier of the packet
        /// </summary>
        public int Identifier { get; set; }
        /// <summary>
        /// Data of the packet
        /// </summary>
        public byte[] PacketData { get; set; }
        /// <summary>
        /// The socket this packet was sent from
        /// </summary>
        public Socket Source { get; set; }
    }
}
