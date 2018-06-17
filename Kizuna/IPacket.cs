namespace Kizuna
{
    /// <summary>
    /// Interface for any packet.
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// The type of the packet
        /// </summary>
        int Identifier { get; }

        /// <summary>
        /// The binary data of the packet
        /// </summary>
        byte[] PacketData { get; }

        /// <summary>
        /// Convert whole packet to byte array
        /// </summary>
        /// <returns></returns>
        byte[] ToByteArray();
    }
}
