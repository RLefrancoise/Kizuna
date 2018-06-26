#include "abstract_packet_builder.hpp"

namespace kizuna
{
	void abstract_packet_builder::register_packet_identifier(int packet_type, int identifier)
	{
		if(type_of_identifiers_.find(packet_type) == type_of_identifiers_.end())
			type_of_identifiers_.emplace(std::make_pair(packet_type, std::vector<int>()));
		
		type_of_identifiers_[packet_type].emplace_back(identifier);
	}

	int abstract_packet_builder::get_type_from_identifier(int identifier) const
	{
		for(auto it = type_of_identifiers_.begin() ; it != type_of_identifiers_.end() ; ++it)
		{
			auto contains = false;
			for(auto identifiers_it = it->second.begin() ; identifiers_it != it->second.end() ; ++identifiers_it)
			{
				if (*identifiers_it == identifier) {
					contains = true;
					break;
				}
			}

			if (contains) return it->first;
		}

		throw std::runtime_error("Packet identifier doesn't match any registered type");
	}

	std::unique_ptr<iincomingpacket> abstract_packet_builder::create_packet(
		kissnet::socket<kissnet::protocol::tcp>* source, const std::vector<unsigned char>& bytes)
	{
		//check if enough bytes
		const auto bytes_available = source->bytes_available();
		if(bytes_available < sizeof(int))
		{
			throw std::runtime_error("Not enough bytes available to read the packet identifier");
		}

		//read packet identifier
		kissnet::buffer<sizeof(int)> packet_identifier_bytes;

		auto [identifier_bytes_read, identifier_is_valid] = source->recv(packet_identifier_bytes);
		if(identifier_bytes_read < sizeof(int))
		{
			throw std::runtime_error("Failed to read packet identifier from packet");
		}

		const int packet_identifier = reinterpret_cast<int>(packet_identifier_bytes.data());

		//get data bytes
		kissnet::buffer<4096> data_bytes;
		auto [bytes_read, is_valid] = source->recv(data_bytes);

		return create_packet_from_data(incoming_packet_info {
			packet_identifier,
			data_bytes.data(),
			source
		});
	}

	std::unique_ptr<iincomingpacket> abstract_packet_builder::create_packet_from_data(const incoming_packet_info& info)
	{
		return packet_factories_[get_type_from_identifier(info.identifier)]->from_packet_info(info);
	}
}

