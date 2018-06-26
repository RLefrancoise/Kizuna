#pragma once
#include <vector>

namespace kizuna
{
	class ipacket
	{
	protected:
		virtual ~ipacket() = default;
	public:
		virtual int identifier() const = 0;
		virtual std::vector<unsigned char> packet_data() const = 0;
		virtual std::vector<unsigned char> to_byte_array() const = 0;
	};
}
