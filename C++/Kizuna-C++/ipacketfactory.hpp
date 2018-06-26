#pragma once

#include "iincomingpacket.hpp"
#include "incoming_packet_info.hpp"

namespace kizuna
{
	class ipacketfactory
	{		
	public:
		virtual ~ipacketfactory() = default;
		virtual std::unique_ptr<iincomingpacket> from_packet_info(const incoming_packet_info& info) = 0;
	};
}
