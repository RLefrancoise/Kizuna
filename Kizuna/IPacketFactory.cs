namespace Kizuna
{
    /// <summary>
    /// Interface for any packet factory
    /// </summary>
    public interface IPacketFactory
    {
        /// <summary>
        /// Get incoming packet from incoming packet info
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        IIncomingPacket FromPacketInfo(IncomingPacketInfo info);
    }
}
