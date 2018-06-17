using System.Collections.Generic;
using System.Net.Sockets;

namespace Kizuna
{
    /// <summary>
    /// Extends Socket class to add packet handling to it
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Send a packet through the given socket.
        /// </summary>
        /// <param name="destination">The socket to send to packet through</param>
        /// <param name="packet">The packet to send</param>
        public static void SendPacket(this Socket destination, IPacket packet)
        {
            byte[] dataBytes = packet.ToByteArray();
            destination.Send(dataBytes, dataBytes.Length, SocketFlags.None);
        }

        /// <summary>
        /// Broadcast a packet to a list of sockets.
        /// </summary>
        /// <param name="destinations">The sockets to broadcast the packet to</param>
        /// <param name="packet">The packet to broadcast</param>
        public static void BroadcastPacket(this IEnumerable<Socket> destinations, IPacket packet)
        {
            foreach(var destination in destinations)
                destination.SendPacket(packet);
        }
    }
}
