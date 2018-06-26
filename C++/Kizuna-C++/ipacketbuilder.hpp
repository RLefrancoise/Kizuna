#pragma once

#include "iincomingpacket.hpp"

namespace kizuna
{
	class ipacketbuilder
	{
	protected:
		virtual ~ipacketbuilder() = default;
	public:
		virtual std::unique_ptr<iincomingpacket> create_packet(kissnet::socket<kissnet::protocol::tcp>* source, const std::vector<unsigned char>& bytes) = 0;
	};
}