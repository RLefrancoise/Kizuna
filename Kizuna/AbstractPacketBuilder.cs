using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Kizuna
{
    /// <summary>
    /// Base class for packet builders.
    /// </summary>
    public abstract class AbstractPacketBuilder : IPacketBuilder
    {
        /// <summary>
        /// Contains packet identifiers for each packet type
        /// </summary>
        protected Dictionary<int, List<int>> TypeOfIdentifiers = new Dictionary<int, List<int>>();

        /// <summary>
        /// Contains factories for each packet type
        /// </summary>
        protected Dictionary<int, IPacketFactory> PacketFactories = new Dictionary<int, IPacketFactory>();

        /// <summary>
        /// Register a packet factory for the given packet type
        /// </summary>
        /// <typeparam name="TPacketFactory">Type of factory</typeparam>
        /// <param name="packetType">Type of the packet</param>
        /// <param name="factory">The factory to register</param>
        protected void RegisterFactory<TPacketFactory>(int packetType, TPacketFactory factory) where TPacketFactory : IPacketFactory
        {
            PacketFactories.Add(packetType, factory);
        }

        /// <summary>
        /// Register a packet identifier for the given packet type
        /// </summary>
        /// <param name="type">Type of packet the identifier is for</param>
        /// <param name="identifier">The identifier of the packet</param>
        protected void RegisterPacketIdentifier(int type, int identifier)
        {
            if(!TypeOfIdentifiers.ContainsKey(type))
                TypeOfIdentifiers.Add(type, new List<int>());

            if(!TypeOfIdentifiers[type].Contains(identifier))
                TypeOfIdentifiers[type].Add(identifier);
        }

        public IIncomingPacket CreatePacket(Socket source, byte[] receivedBytes)
        {
            //Read packet type
            byte[] packetIdentifierBytes = new byte[sizeof(int)];
            Buffer.BlockCopy(receivedBytes, 0, packetIdentifierBytes, 0, sizeof(int));
            int packetIdentifier = BitConverter.ToInt32(packetIdentifierBytes, 0);

            //Get data bytes
            byte[] dataBytes = new byte[receivedBytes.Length - sizeof(int)];
            Buffer.BlockCopy(receivedBytes, sizeof(int), dataBytes, 0, dataBytes.Length);

            //Create packet
            return CreatePacketFromData(new IncomingPacketInfo
            {
                Identifier = packetIdentifier,
                Source   = source,
                PacketData = dataBytes
            });
        }

        protected virtual int GetTypeFromIdentifier(int identifier)
        {
            foreach (var entry in TypeOfIdentifiers)
            {
                if (entry.Value.Contains(identifier))
                {
                    return entry.Key;
                }
            }

            throw new Exception("Packet identifier doesn't match any registered type");
        }

        protected virtual IIncomingPacket CreatePacketFromData(IncomingPacketInfo info)
        {
            return PacketFactories[GetTypeFromIdentifier(info.Identifier)].FromPacketInfo(info);
        }

        /// <summary>
        /// Read available bytes from socket, create the packet from identifier and handle it
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="builder"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IIncomingPacket ReadAndHandlePacketFromSocket(Socket socket, AbstractPacketBuilder builder, object data = null)
        {
            if (socket == null || !socket.Connected || socket.Available == 0) return null;

            byte[] receivedBytes = new byte[socket.Available];
            int received = socket.Receive(receivedBytes, 0, socket.Available, SocketFlags.None);
            if (received == 0)
            {
                return null;
            }

            IIncomingPacket packet = builder.CreatePacket(socket, receivedBytes);
            packet.HandlePacket(data);

            return packet;
        }
    }
}
