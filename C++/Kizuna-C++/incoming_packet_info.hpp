#pragma once

#include "kissnet.hpp"

namespace kizuna
{
	struct incoming_packet_info
	{
		int identifier;
		std::byte* packet_data;
		kissnet::socket<kissnet::protocol::tcp>* source;
	};
}
