#pragma once

#include "ipacket.hpp"
#include "kissnet.hpp"

namespace kizuna
{
	class iincomingpacket : ipacket
	{
	public:
		virtual kissnet::socket<kissnet::protocol::tcp>* source() = 0;
		virtual void handle_packet(void* data) = 0;
	};
}