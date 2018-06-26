#pragma once
#include <vector>
#include "ipacket.hpp"

namespace kizuna
{
	class abstract_packet : ipacket
	{
	public:
		int identifier() const override;
		std::vector<unsigned char> packet_data() const override;
		std::vector<unsigned char> to_byte_array() const override = 0;

	protected:
		int identifier_;
		std::vector<unsigned char> packet_data_;

		explicit abstract_packet(int identifier);
		explicit abstract_packet(int identifier, const std::vector<unsigned char>& data);
		virtual ~abstract_packet();
	};
}


