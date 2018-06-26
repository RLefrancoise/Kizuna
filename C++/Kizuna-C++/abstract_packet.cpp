#include "abstract_packet.hpp"

namespace kizuna
{
	abstract_packet::abstract_packet(int identifier) : identifier_(identifier)
	{
	}

	abstract_packet::abstract_packet(int identifier, const std::vector<unsigned char>& data) : identifier_(identifier), packet_data_(data)
	{
	}

	abstract_packet::~abstract_packet()
	{
	}

	int abstract_packet::identifier() const
	{
		return identifier_;
	}

	std::vector<unsigned char> abstract_packet::packet_data() const
	{
		return packet_data_;
	}
}


