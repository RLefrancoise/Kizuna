using System.Net.Sockets;

namespace Kizuna
{
    /// <summary>
    /// Base interface for a packet that is incoming
    /// </summary>
    public interface IIncomingPacket: IPacket
    {
        /// <summary>
        /// The sender of the packet
        /// </summary>
        Socket Source { get; }

        /// <summary>
        /// Handle incoming packet
        /// </summary>
        /// <param name="data">Some data to use to handle the packet</param>
        void HandlePacket(object data = null);
    }
}
