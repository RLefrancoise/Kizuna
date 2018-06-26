#pragma once

#include "ipacketbuilder.hpp"
#include <map>
#include "ipacketfactory.hpp"

namespace kizuna
{
	class abstract_packet_builder : ipacketbuilder
	{
	protected:
		std::map<int, std::vector<int> > type_of_identifiers_;
		std::map<int, std::unique_ptr<ipacketfactory> > packet_factories_;

		template<typename factory_type>
		void register_factory(int packet_type, factory_type factory);
		void register_packet_identifier(int packet_type, int identifier);
		virtual int get_type_from_identifier(int identifier) const;
		std::unique_ptr<iincomingpacket> create_packet(kissnet::socket<kissnet::protocol::tcp>* source, const std::vector<unsigned char>& bytes) override;
		virtual std::unique_ptr<iincomingpacket> create_packet_from_data(const incoming_packet_info& info);
	};

	template <typename factory_type>
	void abstract_packet_builder::register_factory(int packet_type, factory_type factory)
	{
		packet_factories_.insert(packet_type, factory);
	}

}


