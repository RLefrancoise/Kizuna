using System;

namespace Kizuna
{
    /// <inheritdoc cref="IPacket"/>
    /// <summary>
    /// Base class for packets
    /// </summary>
    public abstract class AbstractPacket : IPacket
    {
        public int Identifier { get; }
        public byte[] PacketData { get; protected set; }

        /// <summary>
        /// Construct a new packet
        /// </summary>
        /// <param name="identifier">Identifier of the packet</param>
        protected AbstractPacket(int identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Construct a new packet
        /// </summary>
        /// <param name="identifier">Identifier of the packet</param>
        /// <param name="data">Data of the packet</param>
        protected AbstractPacket(int identifier, byte[] data)
        {
            Identifier = identifier;
            PacketData = data;
        }

        /// <summary>
        /// Construct a new packet from incoming data
        /// </summary>
        /// <param name="info">Incoming data</param>
        protected AbstractPacket(IncomingPacketInfo info)
        {
            Identifier = info.Identifier;
            PacketData = info.PacketData;
        }

        /// <summary>
        /// Convert packet message to bytes. Returns PacketData by default.
        /// </summary>
        /// <returns>Message converted to bytes</returns>
        protected virtual byte[] MessageToBytes()
        {
            return PacketData;
        }

        public byte[] ToByteArray()
        {
            byte[] message = MessageToBytes();
            int size = sizeof(int) + message.Length;
            byte[] bytes = new byte[size];

            Buffer.BlockCopy(BitConverter.GetBytes(Identifier), 0, bytes, 0, sizeof(int));
            Buffer.BlockCopy(message, 0, bytes, sizeof(int), message.Length);

            return bytes;
        }
    }
}
