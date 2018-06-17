using System.Net.Sockets;

namespace Kizuna
{
    /// <summary>
    /// Interface for any packet builder.
    /// </summary>
    internal interface IPacketBuilder
    {
        /// <summary>
        /// Create an incoming packet from a source and bytes received 
        /// </summary>
        /// <param name="source">Source that received the data bytes</param>
        /// <param name="receivedBytes">The data bytes to build the packet</param>
        /// <returns></returns>
        IIncomingPacket CreatePacket(Socket source, byte[] receivedBytes);
    }
}
